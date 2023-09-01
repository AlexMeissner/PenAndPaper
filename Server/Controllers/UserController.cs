using DataTransfer.Login;
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

        public UserController(SQLDatabase dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int userId)
        {
            try
            {
                var user = await _dbContext.Users.FirstAsync(x => x.Id == userId);
                var payload = new UsersDto(user.Id, user.Username, user.Email);

                return Ok(payload);
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserCredentialsDto userCredentials)
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

                return CreatedAtAction(nameof(Get), user.Id);
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }
        }
    }
}