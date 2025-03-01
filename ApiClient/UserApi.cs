using DataTransfer.Response;

namespace ApiClient;

public interface IUserApi
{
    Task<Response> Login();
    Task<Response> Register();
}

public class UserApi(IRequest request) : IUserApi
{
    public Task<Response> Login()
    {
        throw new NotImplementedException();
    }

    public Task<Response> Register()
    {
        throw new NotImplementedException();
    }
}