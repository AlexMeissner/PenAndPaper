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
        public async Task<IActionResult> RegisterAsync(UserCredentialsDto userCredentials)
        {
            try
            {
                var user = await _dbContext.Users.AddAsync(new()
                {
                    Email = userCredentials.Email,
                    Username = userCredentials.Username,
                    Password = userCredentials.Password
                });

                await _dbContext.SaveChangesAsync();

                return Ok(new LoginDto() { UserId = user.Entity.Id });
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }
        }
    }
}