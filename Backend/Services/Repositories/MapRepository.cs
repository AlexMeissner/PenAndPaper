using System.Net;
using Backend.Database;
using Backend.Database.Models;
using DataTransfer.Grid;
using DataTransfer.Map;
using DataTransfer.Response;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services.Repositories;

public interface IMapRepository
{
    Task<Response<int>> CreateAsync(int campaignId, MapCreationDto payload);
    Task<Response> DeleteAsync(int mapId);
    Task<Response<ActiveMapDto>> GetActiveMapAsync(int campaignId);
    Task<Response<MapDto>> GetAsync(int mapId);
    Task<Response<IEnumerable<MapsDto>>> GetAllAsync(int campaignId);
    Task<Response<MapDto>> UpdateAsync(int mapId, MapUpdateDto payload);
    Task<Response> UpdateActiveMapAsync(int campaignId, ActiveMapUpdateDto payload);
}

public class MapRepository(PenAndPaperDatabase dbContext) : IMapRepository
{
    public async Task<Response<int>> CreateAsync(int campaignId, MapCreationDto payload)
    {
        var map = new Map()
        {
            Name = payload.Name,
            Image = payload.Image,
            IsGridActive = payload.Grid.IsActive,
            GridSize = payload.Grid.Size,
            Script = "",
            CampaignId = campaignId,
        };

        await dbContext.AddAsync(map);
        await dbContext.SaveChangesAsync();

        return new Response<int>(HttpStatusCode.Created, map.Id);
    }

    public async Task<Response> DeleteAsync(int mapId)
    {
        var map = await dbContext.Maps.FindAsync(mapId);

        if (map is null)
        {
            return new Response(HttpStatusCode.NotFound);
        }

        dbContext.Remove(map);
        await dbContext.SaveChangesAsync();

        return new Response(HttpStatusCode.OK);
    }

    public async Task<Response<ActiveMapDto>> GetActiveMapAsync(int campaignId)
    {
        var campaign = await dbContext.Campaigns
            .Include(c => c.Maps)
            .FirstOrDefaultAsync(c => c.Id == campaignId);

        if (campaign is null)
        {
            return new Response<ActiveMapDto>(HttpStatusCode.NotFound);
        }

        var activeMapId = campaign.Maps.FirstOrDefault(m => m.IsActive)?.Id;

        var activeMapDto = new ActiveMapDto(activeMapId);

        return new Response<ActiveMapDto>(HttpStatusCode.OK, activeMapDto);
    }

    public async Task<Response<MapDto>> GetAsync(int mapId)
    {
        var map = await dbContext.Maps.FindAsync(mapId);

        if (map is null)
        {
            return new Response<MapDto>(HttpStatusCode.NotFound);
        }

        var mapDto = new MapDto(map.Name, map.Image, new GridDto(map.IsGridActive, map.GridSize));

        return new Response<MapDto>(HttpStatusCode.OK, mapDto);
    }

    public async Task<Response<IEnumerable<MapsDto>>> GetAllAsync(int campaignId)
    {
        var campaign = await dbContext.Campaigns.FindAsync(campaignId);

        if (campaign is null)
        {
            return new Response<IEnumerable<MapsDto>>(HttpStatusCode.NotFound);
        }

        await dbContext.Entry(campaign).Collection(x => x.Maps).LoadAsync();

        var maps = campaign.Maps.Select(m => new MapsDto(m.Id, m.Name, m.Image));

        return new Response<IEnumerable<MapsDto>>(HttpStatusCode.OK, maps);
    }

    public async Task<Response<MapDto>> UpdateAsync(int mapId, MapUpdateDto payload)
    {
        var map = await dbContext.Maps.FindAsync(mapId);

        if (map is null)
        {
            return new Response<MapDto>(HttpStatusCode.NotFound);
        }

        if (payload.Name is not null)
        {
            map.Name = payload.Name;
        }

        if (payload.Script is not null)
        {
            map.Script = payload.Script;
        }

        if (payload.Grid is not null)
        {
            map.IsGridActive = payload.Grid.IsActive;
            map.GridSize = payload.Grid.Size;
        }

        await dbContext.SaveChangesAsync();

        var mapDto = new MapDto(map.Name, map.Image, new GridDto(map.IsGridActive, map.GridSize));

        return new Response<MapDto>(HttpStatusCode.OK, mapDto);
    }

    public async Task<Response> UpdateActiveMapAsync(int campaignId, ActiveMapUpdateDto payload)
    {
        var campaign = await dbContext.Campaigns
            .Include(c => c.Maps)
            .FirstOrDefaultAsync(c => c.Id == campaignId);

        if (campaign is null) return new Response(HttpStatusCode.NotFound);

        var map = campaign.Maps.FirstOrDefault(m => m.Id == payload.MapId);

        if (map is null) return new Response(HttpStatusCode.NotFound);

        if (campaign.Maps.FirstOrDefault(m => m.IsActive) is { } activeMap)
        {
            activeMap.IsActive = false;
        }

        map.IsActive = true;

        await dbContext.SaveChangesAsync();

        return new Response(HttpStatusCode.OK);
    }
}