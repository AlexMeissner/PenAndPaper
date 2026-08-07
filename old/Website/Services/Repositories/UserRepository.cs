using Microsoft.EntityFrameworkCore;
using Website.Database;
using Website.Database.Models;

namespace Website.Services.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task UpdateProperties(string username, string color);
}

public class UserRepository(IDbContextFactory<PenAndPaperDatabase> dbContextFactory, IUserClaims claims) : IUserRepository
{
    public async Task<List<User>> GetAllAsync()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        return await dbContext.Users.AsNoTracking().ToListAsync();
    }

    public async Task UpdateProperties(string username, string color)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        if (await dbContext.Users.FindAsync(claims.UserId) is { } user)
        {
            user.Username = username;
            user.Color = color;
            await dbContext.SaveChangesAsync();
        }
    }
}
