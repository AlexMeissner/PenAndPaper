using DataTransfer;
using DataTransfer.User;
using Microsoft.EntityFrameworkCore;

namespace Server.Services
{
    public interface IUser
    {
        public Task<ApiResponse<UsersDto>> GetAsync(int userId);
    }

    public class User : IUser
    {
        private readonly SQLDatabase _dbContext;

        public User(SQLDatabase dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApiResponse<UsersDto>> GetAsync(int userId)
        {
            UsersDto payload;

            try
            {
                var user = await _dbContext.Users.FirstAsync(x => x.Id == userId);
                payload = new() { Id = user.Id, Username = user.Username, Email = user.Email };
            }
            catch (Exception exception)
            {
                return ApiResponse<UsersDto>.Failure(new ErrorDetails(ErrorCode.Exception, exception.Message));
            }

            return ApiResponse<UsersDto>.Success(payload);
        }
    }
}