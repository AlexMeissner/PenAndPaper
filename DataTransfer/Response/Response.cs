using System.Net;

namespace DataTransfer.Response;

public sealed class Response(HttpStatusCode statusCode)
{
    public HttpStatusCode StatusCode { get; init; } = statusCode;

    public bool IsSuccess => StatusCode < HttpStatusCode.BadRequest;
}

public sealed class Response<T>
{
    private readonly T? _value;

    public HttpStatusCode StatusCode { get; }

    public Response(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
    }

    public Response(HttpStatusCode statusCode, T value)
    {
        _value = value;
        StatusCode = statusCode;
    }

    public void Match(Action<T> data, Action<HttpStatusCode> statusCode)
    {
        if (StatusCode >= HttpStatusCode.BadRequest || _value is null)
        {
            statusCode(StatusCode);
            return;
        }

        data(_value);
    }

    public TResult Match<TResult>(Func<T, TResult> data, Func<HttpStatusCode, TResult> statusCode)
    {
        if (StatusCode >= HttpStatusCode.BadRequest || _value is null)
        {
            return statusCode(StatusCode);
        }

        return data(_value);
    }
}