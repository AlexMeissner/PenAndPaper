using Backend.Extensions;
using Backend.Services;
using Backend.Services.Repositories;
using DataTransfer.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Backend.Controllers;

[ApiController]
public class UserController(IIdentity identity, IUserRepository userRepository, IConfiguration configuration) : ControllerBase
{
    [HttpPost("sessions")]
    public async Task<IActionResult> Login(LoginDto payload)
    {
        if (await userRepository.ExistsAsync(payload.Email))
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Name, payload.Name),
                new Claim(JwtRegisteredClaimNames.Email, payload.Email),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["JWT:Key"] ?? throw new NullReferenceException("JWT key not found")));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(12),
                signingCredentials: credentials);

            var a = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new AuthenticationTokenDto(a));
        }

        return Unauthorized();
    }

    [HttpGet("user")]
    public async Task<IActionResult> Get()
    {
        var identityClaims = await identity.FromClaimsPrincipal(User);

        if (identityClaims is null) return Unauthorized();

        var response = await userRepository.GetUserPropeties(identityClaims);

        return response.Match<IActionResult>(
            Ok,
            this.StatusCode);
    }

    [HttpPatch("user")]
    public async Task<IActionResult> UpdateUserPropeties(UserPropertiesUpdate payload)
    {
        var identityClaims = await identity.FromClaimsPrincipal(User);

        if (identityClaims is null) return Unauthorized();

        var response = await userRepository.UpdateUserPropeties(identityClaims, payload);

        return this.StatusCode(response.StatusCode);
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
    public async Task<IActionResult> Register(RegisterDto payload)
    {
        var response = await identity.Register(payload);

        return this.StatusCode(response.StatusCode);
    }
}