using DataTransfer.Campaign;
using DataTransfer.Response;
using DataTransfer.User;

namespace ApiClient;

public interface IUserApi
{
    Task<Response<UserProperties>> GetMyself();
    Task<Response<IEnumerable<CampaignUser>>> GetAllAsync();
    Task<Response<AuthenticationTokenDto>> Login(string email, string name);
    Task<Response> Register(string email, string name);
    Task<Response> UpdateMyself(UserPropertiesUpdate payload);
}

public class UserApi(IRequestBuilder requestBuilder) : IUserApi
{
    public Task<Response<UserProperties>> GetMyself()
    {
        return requestBuilder.Path("user").GetAsync<UserProperties>();
    }

    public Task<Response<IEnumerable<CampaignUser>>> GetAllAsync()
    {
        return requestBuilder.Path("users").GetAsync<IEnumerable<CampaignUser>>();
    }

    public Task<Response<AuthenticationTokenDto>> Login(string email, string name)
    {
        var payload = new LoginDto(email, name);
        return requestBuilder.Path("sessions").PostAsync<AuthenticationTokenDto>(payload);
    }

    public Task<Response> Register(string email, string name)
    {
        var payload = new RegisterDto(email, name);
        return requestBuilder.Path("users").PostAsync(payload);
    }

    public Task<Response> UpdateMyself(UserPropertiesUpdate payload)
    {
        return requestBuilder.Path("user").PatchAsync(payload);
    }
}