using DataTransfer;
using DataTransfer.Login;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly IUserAuthentication _userAuthentication;

        public RegisterController(IUserAuthentication userAuthentication)
        {
            _userAuthentication = userAuthentication;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<LoginDto>>> RegisterAsync(UserCredentialsDto userCredentials)
        {
            var response = await _userAuthentication.RegisterAsync(
                userCredentials.Email, 
                userCredentials.Username, 
                userCredentials.Password).
                ConfigureAwait(false);

            return this.SendResponse<LoginDto>(response);
        }
    }
}