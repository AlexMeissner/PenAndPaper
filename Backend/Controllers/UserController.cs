using System.Security.Claims;
using Backend.Extensions;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
public class UserController(IIdentity identity) : ControllerBase
{
    [HttpPost("sessions")]
    public async Task<IActionResult> Login()
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
    public async Task<IActionResult> Register()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var name = User.FindFirst(ClaimTypes.Name)?.Value;

        if (email is null || name is null)
        {
            return Unauthorized();
        }

        var response = await identity.Register(name, email);

        return this.StatusCode(response.StatusCode);
    }
}