using DataTransfer;
using DataTransfer.Login;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<LoginDto>>> Get()
        {
            _logger.LogInformation("LoginController.Get");
            LoginDto t = new()
            {
                State = LoginState.Success,
                UserId = 1
            };
            return new ApiResponse<LoginDto>(t, null);
        }
    }
}
