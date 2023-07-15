using System.Net;

namespace Server.Middleware
{
    public class HttpLogger
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public HttpLogger(RequestDelegate next, ILogger<HttpLogger> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;

            await _next(context);

            var response = context.Response;

            if (response.StatusCode < (int)HttpStatusCode.BadRequest)
            {
                _logger.LogInformation("[{method}] {path}{queryString} - {statusCodeDescription} ({statusCode})",
                    request.Method,
                    request.Path,
                    request.QueryString,
                    (HttpStatusCode)response.StatusCode,
                    response.StatusCode);
            }
            else
            {
                _logger.LogError("[{method}] {path}{queryString} - {statusCodeDescription} ({statusCode})",
                    request.Method,
                    request.Path,
                    request.QueryString,
                    (HttpStatusCode)response.StatusCode,
                    response.StatusCode);
            }
        }
    }
}
