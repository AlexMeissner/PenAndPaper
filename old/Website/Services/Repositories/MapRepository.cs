using Microsoft.EntityFrameworkCore;
using Website.Database;
using Website.Database.Models;
using Website.Events;

namespace Website.Services.Repositories;

public record GridData(bool IsActive, int Size);

public record LocationItem(int Id, string Name, LocationItem[] SubLocations);

public interface IMapRepository
{
    Task CreateAsync(int campaignId, string name, byte[] image);
    Task DeleteAsync(int mapId);
    Task<int?> GetActiveMapIdAsync(int campaignId);
    Task<Map> GetAsync(int mapId);
    Task<List<LocationItem>> GetLocationItems(int campaignId);
    Task<GridData?> GetGrid(int mapId);
    Task<byte[]?> GetImage(int mapId);
    Task SetActiveAsync(int campaignId, int mapId);
    Task SetGridActiveAsync(int campaignId, int mapId, bool isActive);
    Task SetGridSizeAsync(int campaignId, int mapId, int size);
    Task SetNameAsync(int mapId, string name);
    Task SetScriptAsync(int mapId, string script);
}

public class MapRepository(IDbContextFactory<PenAndPaperDatabase> dbContextFactory, ICampaignEventHub campaignEventHub) : IMapRepository
{
    public async Task CreateAsync(int campaignId, string name, byte[] image)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var map = new Map()
        {
            Name = name,
            Image = image,
            IsGridActive = false,
            GridSize = 10,
            Script = "",
            CampaignId = campaignId,
        };

        await dbContext.AddAsync(map);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int mapId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var map = await dbContext.Maps.FindAsync(mapId);

        if (map is null) return;

        dbContext.Remove(map);

        await dbContext.SaveChangesAsync();
    }

    public async Task<int?> GetActiveMapIdAsync(int campaignId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var campaign = await dbContext.Campaigns
            .AsNoTracking()
            .Include(c => c.Maps)
            .FirstOrDefaultAsync(c => c.Id == campaignId);

        if (campaign is null)
        {
            return null;
        }

        return campaign.Maps.FirstOrDefault(m => m.IsActive)?.Id;
    }

    public async Task<Map> GetAsync(int mapId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        return await dbContext.Maps.AsNoTracking().FirstAsync(m => m.Id == mapId);
    }

    public async Task<GridData?> GetGrid(int mapId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var map = await dbContext.Maps.FindAsync(mapId);

        if (map is null) return null;

        return new GridData(map.IsGridActive, map.GridSize);
    }

    public async Task<byte[]?> GetImage(int mapId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var map = await dbContext.Maps.FindAsync(mapId);

        if (map is null) return null;

        return map.Image;
    }

    public async Task<List<LocationItem>> GetLocationItems(int campaignId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var campaign = await dbContext.Campaigns
            .AsNoTracking()
            .Include(c => c.Maps)
            .FirstOrDefaultAsync(c => c.Id == campaignId);

        if (campaign is null) return [];

        return [.. campaign.Maps
            .Select(m => new LocationItem(m.Id, m.Name, []))
            .OrderBy(l=>l.Name)];
    }

    public async Task SetActiveAsync(int campaignId, int mapId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var campaign = await dbContext.Campaigns
            .Include(c => c.Maps)
            .FirstOrDefaultAsync(c => c.Id == campaignId);

        if (campaign is null) return;

        var map = campaign.Maps.FirstOrDefault(m => m.Id == mapId);

        if (map is null) return;

        if (campaign.Maps.FirstOrDefault(m => m.IsActive) is { } activeMap)
        {
            activeMap.IsActive = false;
        }

        map.IsActive = true;

        await dbContext.SaveChangesAsync();

        campaignEventHub.ForCampaign(campaignId).Publish(new MapChangedEvent(mapId));
    }

    public async Task SetGridActiveAsync(int campaignId, int mapId, bool isActive)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        if (await dbContext.Maps.FindAsync(mapId) is { } map)
        {
            map.IsGridActive = isActive;
            await dbContext.SaveChangesAsync();

            campaignEventHub
                .ForCampaign(campaignId)
                .Publish(new GridChangedEvent(map.IsGridActive, map.GridSize));
        }
    }

    public async Task SetGridSizeAsync(int campaignId, int mapId, int size)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        if (await dbContext.Maps.FindAsync(mapId) is { } map)
        {
            map.GridSize = size;
            await dbContext.SaveChangesAsync();

            campaignEventHub
                .ForCampaign(campaignId)
                .Publish(new GridChangedEvent(map.IsGridActive, map.GridSize));
        }
    }

    public async Task SetNameAsync(int mapId, string name)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var map = await dbContext.Maps.FindAsync(mapId);

        if (map is null) return;

        map.Name = name;

        await dbContext.SaveChangesAsync();
    }

    public async Task SetScriptAsync(int mapId, string script)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var map = await dbContext.Maps.FindAsync(mapId);

        if (map is null) return;

        map.Script = script;

        await dbContext.SaveChangesAsync();
    }
}
