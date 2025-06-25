namespace ApiClient;

public interface ITokenProvider
{
    public Task<string> GetToken();
}