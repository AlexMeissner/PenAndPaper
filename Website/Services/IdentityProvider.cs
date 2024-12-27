using DataTransfer.Login;
using DataTransfer.User;
using Microsoft.AspNetCore.Components.Authorization;
using Website.Services.API;
using static Website.Services.ServiceExtension;
using HttpRequest = Website.Services.API.HttpRequest;

namespace Website.Services;

public interface IIdentityProvider
{
    public Task<string> GetEmailAsync();
    public Task<string> GetNameAsync();
    public Task<UsersDto> GetUserAsync();
}

[ScopedService]
public class IdentityProvider(
    ILogger<IdentityProvider> logger,
    IEndPointProvider endPointProvider,
    ITokenProvider tokenProvider,
    AuthenticationStateProvider authenticationStateProvider) : IIdentityProvider
{
    private readonly HttpRequest _userRequest = new(endPointProvider.BaseURL + "User", tokenProvider);
    private readonly HttpRequest _loginRequest = new(endPointProvider.BaseURL + "Login", tokenProvider);

    private UsersDto? _user;
    private string? _email;
    private string? _name;

    public async Task<string> GetEmailAsync()
    {
        if (_email is not null) return _email;

        var authenticationState = await authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authenticationState.User;

        _email = user.FindFirst(claim => claim.Type.EndsWith("emailaddress"))?.Value;

        return _email ?? throw new NullReferenceException("Email address not found");
    }

    public async Task<string> GetNameAsync()
    {
        if (_name is not null) return _name;

        var authenticationState = await authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authenticationState.User;

        _name = user.FindFirst(claim => claim.Type.EndsWith("givenname"))?.Value;

        return _name ?? throw new NullReferenceException("Email address not found");
    }

    public async Task<UsersDto> GetUserAsync()
    {
        if (_user is not null) return _user;

        _user = await Login(false);

        if (_user is not null) return _user;

        await Register();
        _user = await Login(true);

        if (_user is not null) return _user;

        throw new Exception("Could not retrieve user id");
    }

    private async Task<UsersDto?> Login(bool logError)
    {
        var email = await GetEmailAsync();
        var name = await GetNameAsync();

        var payload = new UserCredentialsDto(email, name, "");
        var response = await _loginRequest.PostAsync<LoginDto>(payload);

        var userId = response.Match<int?>(
            login => login.UserId,
            statusCode =>
            {
                if (logError)
                {
                    // ToDo: Improve
                    logger.LogError("Login failed ({statusCode})", statusCode);
                }

                return null;
            });

        return userId is null ? null : new UsersDto((int)userId, name, email);
    }

    private async Task Register()
    {
        var email = await GetEmailAsync();
        var name = await GetNameAsync();

        var payload = new UserCredentialsDto(email, name, "");
        await _userRequest.PostAsync(payload);
    }
}