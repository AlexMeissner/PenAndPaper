using Backend.Extensions;
using Backend.Services;
using Backend.Services.Repositories;
using DataTransfer.User;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
public class UserController(IIdentity identity, IUserRepository userRepository) : ControllerBase
{
    [HttpPost("sessions")]
    public async Task<IActionResult> Login(LoginDto _)
    {
        var identityClaims = await identity.FromClaimsPrincipal(User);

        return identityClaims is null ? Unauthorized() : Ok();
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAll()
    {
        var identityClaims = await identity.FromClaimsPrincipal(User);

        if (identityClaims is null) return Unauthorized();

        var users = userRepository.GetAll(identityClaims);

        return users.Match<IActionResult>(
            Ok,
            this.StatusCode);
    }

    [HttpPost("users")]
    public async Task<IActionResult> Register(RegisterDto _)
    {
        var response = await identity.Register(User);

        return this.StatusCode(response.StatusCode);
    }
}