using DataTransfer.Campaign;
using DataTransfer.Response;
using DataTransfer.User;

namespace ApiClient;

public interface IUserApi
{
    Task<Response<int>> GetMyId();
    Task<Response<IEnumerable<CampaignUser>>> GetAllAsync();
    Task<Response> Login();
    Task<Response> Register();
}

public class UserApi(IRequestBuilder requestBuilder) : IUserApi
{
    public Task<Response<int>> GetMyId()
    {
        return requestBuilder.Path("user").GetAsync<int>();
    }

    public Task<Response<IEnumerable<CampaignUser>>> GetAllAsync()
    {
        return requestBuilder.Path("users").GetAsync<IEnumerable<CampaignUser>>();
    }

    public Task<Response> Login()
    {
        var payload = new LoginDto();
        return requestBuilder.Path("sessions").PostAsync(payload);
    }

    public Task<Response> Register()
    {
        var payload = new RegisterDto();
        return requestBuilder.Path("users").PostAsync(payload);
    }
}