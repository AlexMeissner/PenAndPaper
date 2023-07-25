using DataTransfer.User;
using System.Threading.Tasks;
using static Client.Services.ServiceExtension;

namespace Client.Services.API
{
    public interface IUserApi
    {
        public Task<HttpResponse<UsersDto>> GetAsync(int userId);
    }

    [TransistentService]
    public class UserApi : IUserApi
    {
        private readonly HttpRequest _request;

        public UserApi(IEndPointProvider endPointProvider)
        {
            _request = new(endPointProvider.BaseURL + "User");
        }

        public Task<HttpResponse<UsersDto>> GetAsync(int userId)
        {
            return _request.GetAsync<UsersDto>($"userId={userId}");
        }
    }
}