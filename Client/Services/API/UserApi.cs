using DataTransfer.Login;
using DataTransfer.User;
using System.Threading.Tasks;
using static Client.Services.ServiceExtension;

namespace Client.Services.API
{
    public interface IUserApi
    {
        public Task<HttpResponse<UsersDto>> GetAsync(int userId);
        public Task<HttpResponse<LoginDto>> LoginAsync(UserCredentialsDto payload);
        public Task<HttpResponse> RegisterAsync(UserCredentialsDto payload);
    }

    [TransistentService]
    public class UserApi : IUserApi
    {
        private readonly HttpRequest _userRequest;
        private readonly HttpRequest _loginRequest;

        public UserApi(IEndPointProvider endPointProvider)
        {
            _userRequest = new(endPointProvider.BaseURL + "User");
            _loginRequest = new(endPointProvider.BaseURL + "Login");
        }

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
