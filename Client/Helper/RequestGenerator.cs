using DataTransfer;
using Flurl.Http;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client.Helper
{
    internal static class RequestGenerator
    {
        private static IFlurlRequest GenerateRequest(string url)
        {
            IFlurlRequest request = new FlurlRequest(url);
            return request;
        }

        private static ApiResponse ErrorGenerator(ErrorDetails error)
        {
            return ApiResponse.Failure(error);
        }

        private static ApiResponse<T> ErrorGenerator<T>(ErrorDetails error)
        {
            return ApiResponse<T>.Failure(error);
        }

        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private static async Task<T> ProcessResponseAsync<T>(
            HttpResponseMessage response,
            Func<ErrorDetails, T> errorGenerator)
            where T : ApiResponse
        {
            if (response.Content.Headers.ContentLength == 0)
            {
                var error = new ErrorDetails(ErrorCode.NoContent, string.Format("{0} - {1}", response.StatusCode, response.ReasonPhrase));
                return errorGenerator(error);
            }

            await using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

            T? apiResponse;

            try
            {
                apiResponse = await JsonSerializer.DeserializeAsync<T>(stream, SerializerOptions).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                var error = new ErrorDetails(ErrorCode.Exception, exception.Message);
                return errorGenerator(error);
            }

            if (apiResponse is null)
            {
                var error = new ErrorDetails(ErrorCode.Initialization, "API response could not be initialized.");
                return errorGenerator(error);
            }

            if (apiResponse.Error is not null && response.Headers.Contains("X-Trace-Identifier"))
            {
                apiResponse.Error.TraceIdentifier = response.Headers.GetValues("X-Trace-Identifier").FirstOrDefault() ?? string.Empty;
            }

            return apiResponse;
        }

        private static async Task<ErrorDetails> GetExceptionErrorDetails(Exception exception)
        {
            if (exception is FlurlHttpException ex)
            {
                if (ex.Call.Response is null)
                {
                    return new ErrorDetails(ErrorCode.Exception, ex.Message);
                }

                if (ex.Call.Response is { StatusCode: (int)HttpStatusCode.Unauthorized })
                {
                    return new ErrorDetails(ErrorCode.Unauthorized, ex.Message);
                }

                try
                {
                    var response = await ex.GetResponseJsonAsync<ApiResponse>().ConfigureAwait(false);

                    if (response.Error is not null)
                    {
                        return new ErrorDetails(response.Error.Code, response.Error.Message);
                    }
                }
                catch { }
            }

            return new ErrorDetails(ErrorCode.Exception, exception.Message);
        }

        private static async Task<ApiResponse> HandleException(Exception exception)
        {
            ApiResponse response;

            if (exception is FlurlHttpException httpException)
            {
                var content = await httpException.GetResponseJsonAsync().ConfigureAwait(false);

                if (JsonSerializer.Deserialize<ApiResponse>(content, SerializerOptions) is { } deserialized)
                {
                    response = deserialized;
                }
                else
                {
                    response = ApiResponse.Failure(new ErrorDetails(ErrorCode.Exception, httpException.Message));
                }
            }
            else
            {
                var error = await GetExceptionErrorDetails(exception);
                response = ApiResponse.Failure(error);
            }

            return response;
        }

        public static async Task<ApiResponse<T>> HandleException<T>(Exception exception)
        {
            var error = await GetExceptionErrorDetails(exception);
            return ApiResponse<T>.Failure(error);
        }

        public static async Task<ApiResponse<T>> GetAsync<T>(this string url)
        {
            var request = GenerateRequest(url);

            try
            {
                var response = await request.GetAsync().ConfigureAwait(false);
                return await ProcessResponseAsync(response.ResponseMessage, ErrorGenerator<T>).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                return await HandleException<T>(exception).ConfigureAwait(false);
            }
        }

        public static async Task<ApiResponse> PostAsync(this string url, object payload)
        {
            var request = GenerateRequest(url);

            try
            {
                var response = await request.PostJsonAsync(payload).ConfigureAwait(false);
                return await ProcessResponseAsync(response.ResponseMessage, ErrorGenerator).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                return await HandleException(exception).ConfigureAwait(false);
            }
        }

        public static async Task<ApiResponse<T>> PostAsync<T>(this string url, object payload)
        {
            var request = GenerateRequest(url);

            try
            {
                var response = await request.PostJsonAsync(payload).ConfigureAwait(false);
                return await ProcessResponseAsync(response.ResponseMessage, ErrorGenerator<T>).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                return await HandleException<T>(exception).ConfigureAwait(false);
            }
        }
    }
}