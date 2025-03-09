using System.Security.Claims;
using Backend.Extensions;
using Backend.Services;
using DataTransfer.User;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
public class UserController(IIdentity identity) : ControllerBase
{
    [HttpPost("sessions")]
    public async Task<IActionResult> Login(LoginDto _)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        if (email is null)
        {
            return Unauthorized();
        }

        var response = await identity.Login(email);

        return this.StatusCode(response.StatusCode);
    }

    [HttpPost("users")]
    public async Task<IActionResult> Register(RegisterDto _)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var name = User.FindFirst(ClaimTypes.GivenName)?.Value;

        if (email is null || name is null)
        {
            return Unauthorized();
        }

        var response = await identity.Register(email, name);

        return this.StatusCode(response.StatusCode);
    }
}