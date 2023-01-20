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
        private readonly IEndPointProvider _endPointProvider;

        public UserApi(IEndPointProvider endPointProvider)
        {
            _endPointProvider = endPointProvider;
        }

        public Task<ApiResponse<UsersDto>> GetAsync(int userId)
        {
            string url = _endPointProvider.BaseURL + $"User?userId={userId}";
            return url.GetAsync<UsersDto>();
        }
    }
}