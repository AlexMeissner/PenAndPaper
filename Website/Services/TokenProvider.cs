namespace Website.Services;

public interface ITokenProvider
{
    public string GetTokenAsync();
}

[ServiceExtension.ScopedService]
public class TokenProvider(IHttpContextAccessor httpContextAccessor) : ITokenProvider
{
    public string GetTokenAsync()
    {
        if (httpContextAccessor.HttpContext is { User.Identity.IsAuthenticated: true } httpContext)
        {
            return httpContext.User.FindFirst(c => c.Type == "id_token")?.Value
                   ?? throw new Exception("Access token could not be reteived");
        }

        throw new NullReferenceException("Http Context could not be instantiated");
    }
}