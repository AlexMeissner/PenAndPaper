using DataTransfer;
using DataTransfer.Login;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IUserAuthentication _userAuthentication;

        public LoginController(IUserAuthentication userAuthentication)
        {
            _userAuthentication = userAuthentication;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<LoginDto>>> LoginAsync(UserCredentialsDto userCredentials)
        {
            var response = await _userAuthentication.LoginAsync(userCredentials.Email, userCredentials.Password).ConfigureAwait(false);
            return this.SendResponse<LoginDto>(response);
        }
    }
}
