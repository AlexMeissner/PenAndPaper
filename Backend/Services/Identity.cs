using System.Net;
using System.Security.Claims;
using Backend.Database;
using Backend.Database.Models;
using DataTransfer.Response;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public record IdentityClaims(User User, string Email, string Name);

public interface IIdentity
{
    public Task<IdentityClaims?> FromClaimsPrincipal(ClaimsPrincipal claimsPrincipal);
    public Task<Response> Register(ClaimsPrincipal claimsPrincipal);
}

public class Identity(PenAndPaperDatabase dbContext) : IIdentity
{
    public async Task<IdentityClaims?> FromClaimsPrincipal(ClaimsPrincipal claimsPrincipal)
    {
        var email = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;

        if (email is null) return null;

        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user is null) return null;

        var name = claimsPrincipal.FindFirst(ClaimTypes.GivenName)?.Value ?? string.Empty;

        return new IdentityClaims(user, email, name);
    }

    public async Task<Response> Register(ClaimsPrincipal claimsPrincipal)
    {
        var email = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;

        if (email is null) return new Response(HttpStatusCode.Unauthorized);

        var name = claimsPrincipal.FindFirst(ClaimTypes.GivenName)?.Value ?? "Incognito";

        var user = new User()
        {
            Email = email,
            Username = name
        };

        await dbContext.AddAsync(user);
        await dbContext.SaveChangesAsync();

        return new Response(HttpStatusCode.Created);
    }
}