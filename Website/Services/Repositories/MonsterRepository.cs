using Microsoft.EntityFrameworkCore;
using Website.Database;
using Website.Database.Models;

namespace Website.Services.Repositories;

public record MonsterItem(int Id, string Name);

public interface IMonsterRepository
{
    List<MonsterItem> GetAllAsync();
    Task<Monster> GetAsync(int monsterId);
}

public class MonsterRepository(IDbContextFactory<PenAndPaperDatabase> dbContextFactory) : IMonsterRepository
{
    public List<MonsterItem> GetAllAsync()
    {
        using var dbContext = dbContextFactory.CreateDbContext();

        return [.. dbContext.Monsters
            .AsNoTracking()
            .Select(m => new MonsterItem(m.Id, m.Name))
            .AsEnumerable()
            .OrderBy(m => m.Name)];
    }

    public async Task<Monster> GetAsync(int monsterId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        return await dbContext.Monsters.AsNoTracking().FirstAsync(m => m.Id == monsterId);
    }
}
