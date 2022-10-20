using Client.Helper;
using DataTransfer;
using DataTransfer.User;
using System.Threading.Tasks;

namespace Client.Services.API
{
    public interface IUserApi
    {
        public Task<ApiResponse<UsersDto>> GetAsync(int userId);
    }

    public class UserApi : IUserApi
    {
        public Task<ApiResponse<UsersDto>> GetAsync(int userId)
        {
            // TODO
            string url = $"https://localhost:7099/User?userId={userId}";
            return url.GetAsync<UsersDto>();
        }
    }
}