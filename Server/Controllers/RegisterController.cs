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
                var user = new DbUser()
                {
                    Email = userCredentials.Email,
                    Username = userCredentials.Username,
                    Password = userCredentials.Password
                };

                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                var routeValue = new { id = user.Id };

                return CreatedAtAction("GetAsync", nameof(UserController), routeValue, user);
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }
        }
    }
}