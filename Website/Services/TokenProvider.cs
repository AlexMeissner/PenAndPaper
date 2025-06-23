using ApiClient;

namespace Website.Services;

internal class TokenProvider : ITokenProvider
{
    private string token = string.Empty;

    public string GetToken()
    {
        return token;
    }

    public void SetToken(string token)
    {
        this.token = token;
    }
}
