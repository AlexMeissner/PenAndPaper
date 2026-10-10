using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Backend.Login;

[ApiController]
[Route("account")]
public class AccountController(IOptions<ReturnUrlOptions> returnUrlOptions) : ControllerBase
{
    [HttpGet("login")]
    [AllowAnonymous]
    public IActionResult Login(string returnUrl)
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = GetSafeReturnUrl(returnUrl)
        };

        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("me")]
    [AllowAnonymous]
    public ActionResult<UserAccountDto> Me()
    {
        if (User.Identity?.IsAuthenticated != true)
        {
            return Unauthorized();
        }

        return new UserAccountDto(
            User.Identity.Name,
            User.FindFirstValue(ClaimTypes.Email),
            User.FindFirstValue("urn:google:picture"));
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return NoContent();
    }

    private string GetSafeReturnUrl(string returnUrl)
    {
        if (string.IsNullOrWhiteSpace(returnUrl))
        {
            return "/";
        }

        if (Url.IsLocalUrl(returnUrl))
        {
            return returnUrl;
        }

        if (Uri.TryCreate(returnUrl, UriKind.Absolute, out var uri) && uri.Scheme == Uri.UriSchemeHttps)
        {
            var origin = uri.GetLeftPart(UriPartial.Authority);
            var ownOrigin = $"{Request.Scheme}://{Request.Host}";

            if (string.Equals(origin, ownOrigin, StringComparison.OrdinalIgnoreCase) ||
                returnUrlOptions.Value.AllowedOrigins.Contains(origin, StringComparer.OrdinalIgnoreCase))
            {
                return uri.ToString();
            }
        }

        return "/";
    }
}
