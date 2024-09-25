using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using static Website.Services.ServiceExtension;

namespace Website.Services;

public interface ITokenProvider
{
    public Task<string> GetTokenAsync();
}

[TransistentService]
public class TokenProvider(IHttpContextAccessor httpContextAccessor, AuthenticationStateProvider provider) : ITokenProvider
{
    public async Task<string> GetTokenAsync()
    {
        if (httpContextAccessor.HttpContext is HttpContext httpContext
            && httpContext.User.Identity?.IsAuthenticated == true)
        {
            var accessToken = await httpContext.GetTokenAsync("access_token");
            var idToken = await httpContext.GetTokenAsync("id_token");
            var tokenType = await httpContext.GetTokenAsync("token_type");
            var expiresAt = await httpContext.GetTokenAsync("expires_at");

            var state = await provider.GetAuthenticationStateAsync();
            var user = state.User;

            if (user.Identity.IsAuthenticated)
            {
                var jwt = user.FindFirst(c => c.Type == "id_token")?.Value;
            }

            return await httpContext.GetTokenAsync("access_token")
                ?? throw new Exception("Access token could not be reteived");
        }

        throw new NullReferenceException("Http Context could not be instantiated");
    }
}
