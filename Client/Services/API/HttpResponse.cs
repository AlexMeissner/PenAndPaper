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

    public readonly struct HttpResponse<TValue>
    {
        private readonly HttpStatusCode _statusCode;
        private readonly TValue? _value;

        private bool IsError => _statusCode >= HttpStatusCode.BadRequest;

        public HttpResponse(HttpStatusCode statusCode, TValue? value)
        {
            _statusCode = statusCode;
            _value = value;
        }

        public void Match(Action<TValue> success) => success(_value!);

        public void Match(Action<TValue> success, Action<HttpStatusCode> failure)
        {
            if (IsError)
            {
                failure(_statusCode);
            }
            else
            {
                success(_value!);
            }
        }
    }
}
