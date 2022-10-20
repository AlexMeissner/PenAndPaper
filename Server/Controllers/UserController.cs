using DataTransfer;
using DataTransfer.User;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUser _user;

        public UserController(IUser user)
        {
            _user = user;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<UsersDto>>> GetAsync(int userId)
        {
            var response = await _user.GetAsync(userId);
            return this.SendResponse<UsersDto>(response);
        }
    }
}