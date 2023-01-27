using DataTransfer;
using DataTransfer.Login;
using Microsoft.AspNetCore.Mvc;
using Server.Database;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly SQLDatabase _dbContext;

        public RegisterController(SQLDatabase dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<LoginDto>>> RegisterAsync(UserCredentialsDto userCredentials)
        {
            try
            {
                var entry = await _dbContext.Users.AddAsync(new()
                {
                    Email = userCredentials.Email,
                    Username = userCredentials.Username,
                    Password = userCredentials.Password
                });

                await _dbContext.SaveChangesAsync();

                var payload = new LoginDto() { UserId = entry.Entity.Id };
                var response = ApiResponse<LoginDto>.Success(payload);

                return this.SendResponse<LoginDto>(response);
            }
            catch (Exception exception)
            {
                var response = ApiResponse<LoginDto>.Failure(new ErrorDetails(ErrorCode.Exception, exception.Message));
                return this.SendResponse<LoginDto>(response);
            }
        }
    }
}