using DataTransfer;
using DataTransfer.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Database;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly SQLDatabase _dbContext;
        private readonly ILogger<LoginController> _logger;

        public LoginController(SQLDatabase dbContext, ILogger<LoginController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<LoginDto>>> LoginAsync(UserCredentialsDto userCredentials)
        {
            _logger.LogInformation(nameof(LoginAsync));

            try
            {
                var entry = await _dbContext.Users.FirstAsync(x => x.Email == userCredentials.Email && x.Password == userCredentials.Password);

                if (entry is null)
                {
                    return ApiResponse<LoginDto>.Failure(new ErrorDetails(ErrorCode.InvalidLogin, "Email or password incorrect."));
                }

                var payload = new LoginDto() { UserId = entry.Id };
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
