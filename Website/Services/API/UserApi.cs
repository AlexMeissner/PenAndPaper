using DataTransfer.Login;
using DataTransfer.User;
using static Website.Services.ServiceExtension;

namespace Website.Services.API
{
    public interface IUserApi
    {
        public Task<HttpResponse<UsersDto>> GetAsync(int userId);
        public Task<HttpResponse<LoginDto>> LoginAsync(UserCredentialsDto payload);
        public Task<HttpResponse> RegisterAsync(UserCredentialsDto payload);
    }

    [TransistentService]
    public class UserApi(IEndPointProvider endPointProvider) : IUserApi
    {
        private readonly HttpRequest _userRequest = new(endPointProvider.BaseURL + "User");
        private readonly HttpRequest _loginRequest = new(endPointProvider.BaseURL + "Login");

        public Task<HttpResponse<UsersDto>> GetAsync(int userId)
        {
            return _userRequest.GetAsync<UsersDto>($"userId={userId}");
        }

        public Task<HttpResponse<LoginDto>> LoginAsync(UserCredentialsDto payload)
        {
            return _loginRequest.PostAsync<LoginDto>(payload);
        }

        public Task<HttpResponse> RegisterAsync(UserCredentialsDto payload)
        {
            return _userRequest.PostAsync(payload);
        }
    }
}
