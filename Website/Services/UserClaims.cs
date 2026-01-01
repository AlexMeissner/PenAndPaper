using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Website.Database;
using Website.Database.Models;

namespace Website.Services;

public interface IUserClaims
{
    int UserId { get; }

    Task<User> GetUserAsync();
    Task InitializeAsync();
}

public class UserClaims(IHttpContextAccessor httpContextAccessor, IDbContextFactory<PenAndPaperDatabase> dbContextFactory) : IUserClaims
{
    private int? _userId;

    public int UserId => _userId ?? throw new NullReferenceException("Claims not initialized");

    public async Task<User> GetUserAsync()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        return await dbContext.Users.AsNoTracking().FirstAsync(u => u.Id == _userId) ?? throw new NullReferenceException("Claims not initialized");
    }

    public async Task InitializeAsync()
    {
        if (httpContextAccessor.HttpContext is { User.Identity.IsAuthenticated: true } httpContext &&
             httpContext.User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value is { } email &&
             httpContext.User.FindFirst(c => c.Type == ClaimTypes.GivenName)?.Value is { } name)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            var user = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);

            if (user is null)
            {
                const string DefaultColor = "#FF0000";

                user = new User()
                {
                    Email = email,
                    Username = name,
                    Color = DefaultColor
                };

                await dbContext.AddAsync(user);
                await dbContext.SaveChangesAsync();
            }

            _userId = user.Id;
        }
    }
}
