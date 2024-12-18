using Microsoft.AspNetCore.Components.Authorization;
using static Website.Services.ServiceExtension;

namespace Website.Services;

public interface IIdentityProvider
{
    public Task<string> GetEmailAsync();
    public Task<string> GetNameAsync();
    public string GetTokenAsync();
}

[TransistentService]
public class IdentityProvider(
    IHttpContextAccessor httpContextAccessor,
    AuthenticationStateProvider authenticationStateProvider) : IIdentityProvider
{
    public async Task<string> GetEmailAsync()
    {
        var authenticationState = await authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authenticationState.User;

        return user.FindFirst(claim => claim.Type.EndsWith("emailaddress"))?.Value ??
               throw new NullReferenceException("Email address not found");
    }

    public async Task<string> GetNameAsync()
    {
        var authenticationState = await authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authenticationState.User;

        return user.FindFirst(claim => claim.Type.EndsWith("givenname"))?.Value ??
               throw new NullReferenceException("Email address not found");
    }

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