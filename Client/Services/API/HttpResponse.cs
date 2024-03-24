using System;
using System.Net;

namespace Client.Services.API
{
    public readonly struct HttpResponse
    {
        public HttpStatusCode StatusCode { get; init; }

        public bool Failed => StatusCode >= HttpStatusCode.BadRequest;
        public bool Succeded => StatusCode < HttpStatusCode.BadRequest;

        public HttpResponse(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
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
    }
}
