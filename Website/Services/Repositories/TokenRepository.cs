using Microsoft.EntityFrameworkCore;
using Website.Database;
using Website.Database.Models;
using Website.Events;

namespace Website.Services.Repositories;

public record TokenItem(int TokenId, int UserId, byte[] Image, int X, int Y);

public interface ITokenRepository
{
    Task<int?> CreateCharacterAsync(int campaignId, int mapId, int characterId, int x, int y);
    Task<int?> CreateMonsterAsync(int campaignId, int mapId, int monsterId, int x, int y);
    Task DeleteAsync(int tokenId);
    Task<List<TokenItem>> GetAllAsync(int mapId);
    Task MoveAsync(int campaignId, int tokenId, int x, int y);
}

public class TokenRepository(IDbContextFactory<PenAndPaperDatabase> dbContextFactory, ICampaignEventHub campaignEventHub) : ITokenRepository
{
    public async Task<int?> CreateCharacterAsync(int campaignId, int mapId, int characterId, int x, int y)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var character = await dbContext.Characters.FindAsync(characterId);

        if (character is null) return null;

        var map = await dbContext.Maps.FindAsync(mapId);

        if (map is null) return null;

        var token = new CharacterToken()
        {
            X = x,
            Y = y,
            Map = map,
            OwnerId = character.UserId,
            Character = character
        };

        await dbContext.AddAsync(token);
        await dbContext.SaveChangesAsync();

        campaignEventHub
            .ForCampaign(campaignId)
            .Publish(new TokenAddedEvent(token.Id, token.OwnerId, character.Image, x, y));

        return token.Id;
    }

    public async Task<int?> CreateMonsterAsync(int campaignId, int mapId, int monsterId, int x, int y)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var monster = await dbContext.Monsters.FindAsync(monsterId);

        if (monster is null) return null;

        var campaign = await dbContext.Campaigns
            .Include(c => c.Maps)
            .FirstOrDefaultAsync(c => c.Maps.Any(m => m.Id == mapId));

        if (campaign is null) return null;

        var map = await dbContext.Maps.FindAsync(mapId);

        if (map is null) return null;

        var token = new MonsterToken()
        {
            X = x,
            Y = y,
            Map = map,
            OwnerId = campaign.GameMasterId,
            Monster = monster
        };

        await dbContext.AddAsync(token);
        await dbContext.SaveChangesAsync();

        campaignEventHub
            .ForCampaign(campaignId)
            .Publish(new TokenAddedEvent(token.Id, token.OwnerId, monster.Image, x, y));

        return token.Id;
    }

    public async Task DeleteAsync(int tokenId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var token = await dbContext.Tokens.FindAsync(tokenId);

        if (token is null) return;

        dbContext.Remove(token);

        await dbContext.SaveChangesAsync();
    }

    public async Task<List<TokenItem>> GetAllAsync(int mapId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var characterTokens = dbContext.CharacterTokens
            .AsNoTracking()
            .Where(t => t.MapId == mapId)
            .Include(t => t.Character)
            .Select(t => new { t.Id, t.OwnerId, t.X, t.Y, t.Character.Name, t.Character.Image });

        var monsterTokens = dbContext.MonsterTokens
            .AsNoTracking()
            .Where(t => t.MapId == mapId)
            .Include(t => t.Monster)
            .Select(t => new { t.Id, t.OwnerId, t.X, t.Y, t.Monster.Name, t.Monster.Image });

        return await characterTokens
            .Concat(monsterTokens)
            .Select(t => new TokenItem(t.Id, t.OwnerId, t.Image, t.X, t.Y)).ToListAsync();
    }

    public async Task MoveAsync(int campaignId, int tokenId, int x, int y)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var token = await dbContext.Tokens.FindAsync(tokenId);

        if (token is null) return;

        token.X = x;
        token.Y = y;

        await dbContext.SaveChangesAsync();

        campaignEventHub
            .ForCampaign(campaignId)
            .Publish(new TokenMovedEvent(token.Id, x, y));
    }
}
