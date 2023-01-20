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
        private readonly IEndPointProvider _endPointProvider;

        public AuthenticationApi(IEndPointProvider endPointProvider)
        {
            _endPointProvider = endPointProvider;
        }

        public Task<ApiResponse<LoginDto>> LoginAsync(UserCredentialsDto payload)
        {
            string url = _endPointProvider.BaseURL + "Login";
            return url.PostAsync<LoginDto>(payload);
        }

        public Task<ApiResponse<LoginDto>> RegisterAsync(UserCredentialsDto payload)
        {
            string url = _endPointProvider.BaseURL + "Register";
            return url.PostAsync<LoginDto>(payload);
        }
    }
}