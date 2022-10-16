using Client.Helper;
using DataTransfer;
using DataTransfer.Login;
using System.Threading.Tasks;

namespace Client.Services.API
{
    public interface IAuthenticationApi
    {
        public Task<ApiResponse<LoginDto>> LoginAsync(UserCredentialsDto payload);
        public Task<ApiResponse<LoginDto>> RegisterAsync(UserCredentialsDto payload);
    }

    public class AuthenticationApi : IAuthenticationApi
    {
        public Task<ApiResponse<LoginDto>> LoginAsync(UserCredentialsDto payload)
        {
            string url = "https://localhost:7099/Login";
            return url.PostAsync<LoginDto>(payload);
        }

        public Task<ApiResponse<LoginDto>> RegisterAsync(UserCredentialsDto payload)
        {
            string url = "https://localhost:7099/Register";
            return url.PostAsync<LoginDto>(payload);
        }
    }
}