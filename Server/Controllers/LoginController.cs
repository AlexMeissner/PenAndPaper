using DataTransfer.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController(SQLDatabase dbContext) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Login(UserCredentialsDto userCredentials)
        {
            var entry = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == userCredentials.Email && x.Password == userCredentials.Password);

            if (entry is null)
            {
                return Unauthorized();
            }

            var response = new LoginDto(entry.Id);

            return Ok(response);
        }
    }
}
