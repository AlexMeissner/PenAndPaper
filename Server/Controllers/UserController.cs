using DataTransfer.Login;
using DataTransfer.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(SQLDatabase dbContext) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(int userId)
        {
            var user = await dbContext.Users.FindAsync(userId);

            if (user is null)
            {
                return NotFound(userId);
            }

            var payload = new UsersDto(user.Id, user.Username, user.Email);

            return Ok(payload);
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserCredentialsDto userCredentials)
        {
            var user = new User()
            {
                Email = userCredentials.Email,
                Username = userCredentials.Username,
                Password = userCredentials.Password
            };

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), user.Id);
        }
    }
}