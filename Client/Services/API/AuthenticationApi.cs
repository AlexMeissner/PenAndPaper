using DataTransfer.Login;
using System.Threading.Tasks;

namespace Client.Services.API
{
    public interface IAuthenticationApi
    {
        public Task<HttpResponse<LoginDto>> LoginAsync(UserCredentialsDto payload);
        public Task<HttpResponse<LoginDto>> RegisterAsync(UserCredentialsDto payload);
    }

    public class AuthenticationApi : IAuthenticationApi
    {
        private readonly HttpRequest _loginRequest;
        private readonly HttpRequest _registerRequest;

        public AuthenticationApi(IEndPointProvider endPointProvider)
        {
            _loginRequest = new(endPointProvider.BaseURL + "Login");
            _registerRequest = new(endPointProvider.BaseURL + "Register");
        }

        public Task<HttpResponse<LoginDto>> LoginAsync(UserCredentialsDto payload)
        {
            return _loginRequest.PostAsync<LoginDto>(payload);
        }

        public Task<HttpResponse<LoginDto>> RegisterAsync(UserCredentialsDto payload)
        {
            return _registerRequest.PostAsync<LoginDto>(payload);
        }
    }
}