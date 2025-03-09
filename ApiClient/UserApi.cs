using DataTransfer.Response;
using DataTransfer.User;

namespace ApiClient;

public interface IUserApi
{
    Task<Response> Login();
    Task<Response> Register();
}

public class UserApi(IRequestBuilder requestBuilder) : IUserApi
{
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