using DataTransfer.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly SQLDatabase _dbContext;

        public LoginController(SQLDatabase dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserCredentialsDto userCredentials)
        {
            try
            {
                var entry = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == userCredentials.Email && x.Password == userCredentials.Password);

                if (entry is null)
                {
                    return Unauthorized();
                }

                return Ok(new LoginDto(entry.Id));
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }
        }
    }
}
