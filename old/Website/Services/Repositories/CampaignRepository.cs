using Microsoft.EntityFrameworkCore;
using Website.Database;
using Website.Database.Models;

namespace Website.Services.Repositories;

public record CampaignItem(int Id, string Name, string GameMaster, IEnumerable<string> Players, bool IsGameMaster);

public interface ICampaignRepository
{
    Task<int> CreateAsync(string name, IEnumerable<int> playerIds);
    Task<List<CampaignItem>> GetAllAsync();
    Task<Campaign> GetAsync(int campaignId);
    Task UpdateAsync(int campaignId, string name, IEnumerable<int> playerIds);
}

public class CampaignRepository(IDbContextFactory<PenAndPaperDatabase> dbContextFactory, IUserClaims claims) : ICampaignRepository
{
    public async Task<int> CreateAsync(string name, IEnumerable<int> playerIds)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var players = await dbContext.Users.Where(u => playerIds.Contains(u.Id)).ToListAsync();
        var gamemaster = await claims.GetUserAsync();

        var campaign = new Campaign()
        {
            Name = name,
            GameMaster = gamemaster,
            Players = players
        };

        await dbContext.AddAsync(campaign);
        await dbContext.SaveChangesAsync();

        return campaign.Id;
    }

    public async Task<List<CampaignItem>> GetAllAsync()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var me = await claims.GetUserAsync();

        return await dbContext.Campaigns
            .AsNoTracking()
            .Include(c => c.GameMaster)
            .Include(c => c.Players)
            .Where(c => c.GameMaster == me || c.Players.Contains(me))
            .Select(c => new CampaignItem(c.Id, c.Name, c.GameMaster.Username, c.Players.Select(u => u.Username), c.GameMaster == me))
            .ToListAsync();
    }

    public async Task<Campaign> GetAsync(int campaignId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        return await dbContext.Campaigns
            .AsNoTracking()
            .Include(c => c.Players)
            .FirstAsync(c => c.Id == campaignId);
    }

    public async Task UpdateAsync(int campaignId, string name, IEnumerable<int> playerIds)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var campaign = await dbContext.Campaigns.FindAsync(campaignId);

        if (campaign is null) return;

        await dbContext.Entry(campaign).Collection(c => c.Players).LoadAsync();

        var players = dbContext.Users.Where(u => playerIds.Contains(u.Id));

        campaign.Players = [.. players];
        campaign.Name = name;

        await dbContext.SaveChangesAsync();
    }
}
