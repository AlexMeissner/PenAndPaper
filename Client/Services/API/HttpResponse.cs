using System;
using System.Net;

namespace Client.Services.API
{
    public readonly struct HttpResponse(HttpStatusCode statusCode)
    {
        public HttpStatusCode StatusCode { get; init; } = statusCode;

        public bool Failed => StatusCode >= HttpStatusCode.BadRequest;
        public bool Succeded => StatusCode < HttpStatusCode.BadRequest;
    }

    public readonly struct HttpResponse<TValue>(HttpStatusCode statusCode, TValue? value)
    {
        private bool IsError => statusCode >= HttpStatusCode.BadRequest;

        public void Match(Action<TValue> success) => success(value!);

        public void Match(Action<TValue> success, Action<HttpStatusCode> failure)
        {
            if (IsError)
            {
                failure(statusCode);
            }
            else
            {
                success(value!);
            }
        }

        public TResult Match<TResult>(Func<TValue, TResult> success, Func<HttpStatusCode, TResult> failure)
        {
            if (IsError)
            {
                return failure(statusCode);
            }

            return success(value!);
        }
    }
}
