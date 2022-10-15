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

        private static ApiResponse<TModel> ErrorGenerator<TModel>(ErrorDetails error)
        {
            return ApiResponse<TModel>.Failure(error);
        }

        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private static async Task<TResponse> ProcessResponseAsync<TResponse>(
            HttpResponseMessage response,
            Func<ErrorDetails, TResponse> errorGenerator)
            where TResponse : ApiResponse
        {
            if (response.Content.Headers.ContentLength == 0)
            {
                var error = new ErrorDetails(ErrorCode.NoContent, string.Format("{0} - {1}", response.StatusCode, response.ReasonPhrase));
                return errorGenerator(error);
            }

            await using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

            TResponse? apiResponse;

            try
            {
                apiResponse = await JsonSerializer.DeserializeAsync<TResponse>(stream, SerializerOptions).ConfigureAwait(false);
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

        private static async Task<ApiResponse<TResponse>> HandleException<TResponse>(Exception exception)
        {
            var error = await GetExceptionErrorDetails(exception);
            return ApiResponse<TResponse>.Failure(error);
        }

        public static async Task<ApiResponse<TResponse>> GetAsync<TResponse>(this string url)
        {
            var request = GenerateRequest(url);

            try
            {
                var response = await request.GetAsync().ConfigureAwait(false);
                return await ProcessResponseAsync(response.ResponseMessage, ErrorGenerator<TResponse>).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                return await HandleException<TResponse>(exception).ConfigureAwait(false);
            }
        }
    }
}