using ApiClient;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Website.Services;

internal class TokenProvider(ProtectedSessionStorage sessionStorage) : ITokenProvider
{
    private const string TokenKey = "penAndPaperBackendApiToken";

    public async Task<string> GetToken()
    {
        var result = await sessionStorage.GetAsync<string>(TokenKey);
        return result.Success && result.Value is { } token ? token : string.Empty;
    }

    public async Task SetToken(string token)
    {
        await sessionStorage.SetAsync(TokenKey, token);
    }
}
