using Flurl.Http;
using System.Net;
using System.Text.Json;

namespace Website.Services.API;

public class HttpRequest(string endpoint, ITokenProvider tokenProvider)
{
    public async Task<HttpResponse> GetAsync()
    {
        IFlurlRequest request = CreateRequest(endpoint);
        var response = await request.AllowAnyHttpStatus().GetAsync().ConfigureAwait(false);
        return new HttpResponse(response.ResponseMessage.StatusCode);
    }

    public async Task<HttpResponse> GetAsync(string parameters)
    {
        string url = string.Format("{0}?{1}", endpoint, parameters);
        IFlurlRequest request = CreateRequest(url);
        var response = await request.AllowAnyHttpStatus().GetAsync().ConfigureAwait(false);
        return new HttpResponse(response.ResponseMessage.StatusCode);
    }

    public async Task<HttpResponse<T>> GetAsync<T>()
    {
        IFlurlRequest request = CreateRequest(endpoint);
        var response = await request.AllowAnyHttpStatus().GetAsync().ConfigureAwait(false);
        return await ProcessResponseAsync<T>(response.ResponseMessage);
    }

    public async Task<HttpResponse<T>> GetAsync<T>(string parameters)
    {
        string url = string.Format("{0}?{1}", endpoint, parameters);
        IFlurlRequest request = CreateRequest(url);
        var response = await request.AllowAnyHttpStatus().GetAsync().ConfigureAwait(false);
        return await ProcessResponseAsync<T>(response.ResponseMessage);
    }

    public async Task<HttpResponse> PostAsync(object payload)
    {
        IFlurlRequest request = CreateRequest(endpoint);
        var response = await request.AllowAnyHttpStatus().PostJsonAsync(payload).ConfigureAwait(false);
        return new HttpResponse(response.ResponseMessage.StatusCode);
    }

    public async Task<HttpResponse<T>> PostAsync<T>(object payload)
    {
        IFlurlRequest request = CreateRequest(endpoint);
        var response = await request.AllowAnyHttpStatus().PostJsonAsync(payload).ConfigureAwait(false);
        return await ProcessResponseAsync<T>(response.ResponseMessage);
    }

    public async Task<HttpResponse> PutAsync(object payload)
    {
        IFlurlRequest request = CreateRequest(endpoint);
        var response = await request.AllowAnyHttpStatus().PutJsonAsync(payload).ConfigureAwait(false);
        return new HttpResponse(response.ResponseMessage.StatusCode);
    }

    public async Task<HttpResponse<T>> PutAsync<T>(object payload)
    {
        IFlurlRequest request = CreateRequest(endpoint);
        var response = await request.AllowAnyHttpStatus().PutJsonAsync(payload).ConfigureAwait(false);
        return await ProcessResponseAsync<T>(response.ResponseMessage);
    }

    public async Task<HttpResponse> DeleteAsync()
    {
        IFlurlRequest request = CreateRequest(endpoint);
        var response = await request.AllowAnyHttpStatus().DeleteAsync().ConfigureAwait(false);
        return new HttpResponse(response.ResponseMessage.StatusCode);
    }

    public async Task<HttpResponse> DeleteAsync(string parameters)
    {
        string url = string.Format("{0}?{1}", endpoint, parameters);
        IFlurlRequest request = CreateRequest(url);
        var response = await request.AllowAnyHttpStatus().DeleteAsync().ConfigureAwait(false);
        return new HttpResponse(response.ResponseMessage.StatusCode);
    }

    public async Task<HttpResponse<T>> DeleteAsync<T>()
    {
        IFlurlRequest request = CreateRequest(endpoint);
        var response = await request.AllowAnyHttpStatus().DeleteAsync().ConfigureAwait(false);
        return await ProcessResponseAsync<T>(response.ResponseMessage);
    }

    public async Task<HttpResponse<T>> DeleteAsync<T>(string parameters)
    {
        string url = string.Format("{0}?{1}", endpoint, parameters);
        IFlurlRequest request = CreateRequest(url);
        var response = await request.AllowAnyHttpStatus().DeleteAsync().ConfigureAwait(false);
        return await ProcessResponseAsync<T>(response.ResponseMessage);
    }

    private FlurlRequest CreateRequest(string url)
    {
        var token = tokenProvider.GetTokenAsync();
        return new FlurlRequest(url).WithOAuthBearerToken(token);
    }

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private static async Task<HttpResponse<T>> ProcessResponseAsync<T>(HttpResponseMessage response)
    {
        if (response.StatusCode < HttpStatusCode.BadRequest)
        {
            await using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

            T? value = await JsonSerializer.DeserializeAsync<T>(stream, SerializerOptions).ConfigureAwait(false);

            return new HttpResponse<T>(response.StatusCode, value);
        }

        return new HttpResponse<T>(response.StatusCode, default);
    }
}
