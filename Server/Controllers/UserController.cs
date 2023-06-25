using DataTransfer;
using DataTransfer.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Database;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly SQLDatabase _dbContext;
        private readonly ILogger<UserController> _logger;

        public UserController(SQLDatabase dbContext, ILogger<UserController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<UsersDto>>> GetAsync(int userId)
        {
            _logger.LogInformation(nameof(GetAsync));

            try
            {
                var user = await _dbContext.Users.FirstAsync(x => x.Id == userId);
                var payload = new UsersDto() { Id = user.Id, Username = user.Username, Email = user.Email };
                var response = ApiResponse<UsersDto>.Success(payload);
                return this.SendResponse<UsersDto>(response);
            }
            catch (Exception exception)
            {
                return ApiResponse<UsersDto>.Failure(new ErrorDetails(ErrorCode.Exception, exception.Message));
            }
        }
    }
}