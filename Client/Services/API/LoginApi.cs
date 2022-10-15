using Client.Helper;
using DataTransfer;
using DataTransfer.Login;
using System.Threading.Tasks;

namespace Client.Services.API
{
    public interface ILoginApi
    {
        Task<ApiResponse<LoginDto>> Get();
    }

    public class LoginApi : ILoginApi
    {
        public Task<ApiResponse<LoginDto>> Get()
        {
            string url = "https://localhost:7099/Login";
            return url.GetAsync<LoginDto>();
        }

    }
}