using DataTransfer;
using DataTransfer.Login;
using Microsoft.EntityFrameworkCore;

namespace Server.Services
{
    public interface IUserAuthentication
    {
        public Task<ApiResponse<LoginDto>> LoginAsync(string email, string password);
        public Task<ApiResponse<LoginDto>> RegisterAsync(string email, string username, string password);
    }

    public class UserAuthentication : IUserAuthentication
    {
        private readonly SQLDatabase _dbContext;

        public UserAuthentication(SQLDatabase dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApiResponse<LoginDto>> LoginAsync(string email, string password)
        {
            try
            {
                var entry = await _dbContext.Users.FirstAsync(x => x.Email == email && x.Password == password);

                if (entry is null)
                {
                    return ApiResponse<LoginDto>.Failure(new ErrorDetails(ErrorCode.InvalidLogin, "Email or password incorrect."));
                }

                LoginDto payload = new() { UserId = entry.Id };

                return ApiResponse<LoginDto>.Success(payload);
            }
            catch (Exception exception)
            {
                return ApiResponse<LoginDto>.Failure(new ErrorDetails(ErrorCode.Exception, exception.Message));
            }
        }

        public async Task<ApiResponse<LoginDto>> RegisterAsync(string email, string username, string password)
        {
            try
            {
                var entry = await _dbContext.Users.AddAsync(new() { Email = email, Username = username, Password = password });
                await _dbContext.SaveChangesAsync();

                LoginDto payload = new() { UserId = entry.Entity.Id };

                return ApiResponse<LoginDto>.Success(payload);
            }
            catch (Exception exception)
            {
                return ApiResponse<LoginDto>.Failure(new ErrorDetails(ErrorCode.Exception, exception.Message));
            }
        }
    }
}