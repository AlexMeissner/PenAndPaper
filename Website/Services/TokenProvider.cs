using static Website.Services.ServiceExtension;

namespace Website.Services;

public interface ITokenProvider
{
    public string GetTokenAsync();
}

[TransistentService]
public class TokenProvider(IHttpContextAccessor httpContextAccessor) : ITokenProvider
{
    public string GetTokenAsync()
    {
        if (httpContextAccessor.HttpContext is HttpContext httpContext
            && httpContext.User.Identity?.IsAuthenticated == true)
        {
            return httpContext.User.FindFirst(c => c.Type == "id_token")?.Value
                ?? throw new Exception("Access token could not be reteived");
        }

        throw new NullReferenceException("Http Context could not be instantiated");
    }
}
