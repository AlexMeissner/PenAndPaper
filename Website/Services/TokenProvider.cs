using ApiClient;

namespace Website.Services;

public class TokenProvider(IHttpContextAccessor httpContextAccessor) : ITokenProvider
{
    public string GetToken()
    {
        if (httpContextAccessor.HttpContext is { User.Identity.IsAuthenticated: true } httpContext)
        {
            return httpContext.User.FindFirst(c => c.Type == "id_token")?.Value
                   ?? throw new Exception("Access token could not be retrieved");
        }

        throw new NullReferenceException("Http Context could not be instantiated");
    }
}