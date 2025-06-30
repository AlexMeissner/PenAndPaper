using DataTransfer.Response;
using Flurl.Http;
using System.Net;
using System.Text.Json;

namespace ApiClient;

public interface IRequestBuilder
{
    IRequest Path(params object[] segments);
}

public class RequestBuilder(IEndPointProvider endPointProvider, ITokenProvider tokenProvider) : IRequestBuilder
{
    public IRequest Path(params object[] segments)
    {
        return new Request(endPointProvider, tokenProvider, segments);
    }
}

public interface IRequest
{
    Task<IRequest> Query(string name, object value);

    Task<Response> DeleteAsync();
    Task<Response<T>> GetAsync<T>();
    Task<Response> PatchAsync(object payload);
    Task<Response> PostAsync(object payload);
    Task<Response<T>> PostAsync<T>(object payload);
    Task<Response> PutAsync(object payload);
    Task<Response<T>> PutAsync<T>(object payload);
}

public class Request(IEndPointProvider endPointProvider, ITokenProvider tokenProvider, params object[] segments)
    : IRequest
{
    public async Task<IRequest> Query(string name, object value)
    {
        var request = await CreateRequest();
        request.AppendQueryParam(name, value);
        return this;
    }

    public async Task<Response> DeleteAsync()
    {
        var request = await CreateRequest();
        var response = await request.DeleteAsync().ConfigureAwait(false);
        return new Response(response.ResponseMessage.StatusCode);
    }

    public async Task<Response<T>> GetAsync<T>()
    {
        var request = await CreateRequest();
        var response = await request.GetAsync().ConfigureAwait(false);
        return await DeserializeBodyAsync<T>(response.ResponseMessage).ConfigureAwait(false);
    }

    public async Task<Response> PatchAsync(object payload)
    {
        var request = await CreateRequest();
        var response = await request.PatchJsonAsync(payload).ConfigureAwait(false);
        return new Response(response.ResponseMessage.StatusCode);
    }

    public async Task<Response> PostAsync(object payload)
    {
        var request = await CreateRequest();
        var response = await request.PostJsonAsync(payload).ConfigureAwait(false);
        return new Response(response.ResponseMessage.StatusCode);
    }

    public async Task<Response<T>> PostAsync<T>(object payload)
    {
        var request = await CreateRequest();
        var response = await request.PostJsonAsync(payload).ConfigureAwait(false);
        return await DeserializeBodyAsync<T>(response.ResponseMessage).ConfigureAwait(false);
    }

    public async Task<Response> PutAsync(object payload)
    {
        var request = await CreateRequest();
        var response = await request.PutJsonAsync(payload).ConfigureAwait(false);
        return new Response(response.ResponseMessage.StatusCode);
    }

    public async Task<Response<T>> PutAsync<T>(object payload)
    {
        var request = await CreateRequest();
        var response = await request.PutJsonAsync(payload).ConfigureAwait(false);
        return await DeserializeBodyAsync<T>(response.ResponseMessage).ConfigureAwait(false);
    }

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private static async Task<Response<T>> DeserializeBodyAsync<T>(HttpResponseMessage response)
    {
        if (response.StatusCode >= HttpStatusCode.BadRequest)
        {
            return new Response<T>(response.StatusCode);
        }

        await using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

        var value = await JsonSerializer.DeserializeAsync<T>(stream, SerializerOptions).ConfigureAwait(false);

        return new Response<T>(response.StatusCode, value!);
    }

    private async Task<IFlurlRequest> CreateRequest()
    {
        var token = await tokenProvider.GetToken();

        return new FlurlRequest(endPointProvider.BaseUrl)
            .WithOAuthBearerToken(token)
            .AllowAnyHttpStatus()
            .AppendPathSegments(segments);
    }
}