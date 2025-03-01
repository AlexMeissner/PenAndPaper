using System.Net;
using Backend.Database;
using Backend.Database.Models;
using DataTransfer.Campaign;
using DataTransfer.Response;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services.Repositories;

public interface ICampaignRepository
{
    Task<Response<int>> CreateAsync(CampaignCreationDto payload);
    Task<Response<CampaignDto>> GetAsync(int id);
    Response<IEnumerable<CampaignsDto>> GetAll();
    Task<Response> UpdateAsync(int id, CampaignUpdateDto payload);
}

public class CampaignRepository(PenAndPaperDatabase dbContext, IIdentity identity) : ICampaignRepository
{
    public async Task<Response<int>> CreateAsync(CampaignCreationDto payload)
    {
        var campaign = new Campaign()
        {
            Name = payload.Name,
            GameMaster = identity.User,
        };

        await dbContext.AddAsync(campaign);
        await dbContext.SaveChangesAsync();

        return new Response<int>(HttpStatusCode.Created, campaign.Id);
    }

    public async Task<Response<CampaignDto>> GetAsync(int id)
    {
        var campaign = await dbContext.Campaigns.FindAsync(id);

        if (campaign is null)
        {
            return new Response<CampaignDto>(HttpStatusCode.NotFound);
        }

        await dbContext.Entry(campaign).Collection(c => c.Players).LoadAsync();
        var players = campaign.Players.Select(p => new CampaignUser(p.Id, p.Username)).ToList();

        var playerIds = players.Select(p => p.Id);
        var uninvitedUsers = dbContext.Users
            .Where(u => !playerIds.Contains(u.Id))
            .Select(u => new CampaignUser(u.Id, u.Username));

        // ToDo: Compute IsGameMaster
        var payload = new CampaignDto(campaign.Name, players, uninvitedUsers, true);

        return new Response<CampaignDto>(HttpStatusCode.OK, payload);
    }

    public Response<IEnumerable<CampaignsDto>> GetAll()
    {
        var campaigns = dbContext.Campaigns
            .Include(c => c.GameMaster)
            .Include(c => c.Players)
            .Select(c =>
                new CampaignsDto(c.Id, c.Name, c.GameMaster.Username, c.Players.Select(u => u.Username), true));
        // ToDo: Fill IsGameMaster correctly

        return new Response<IEnumerable<CampaignsDto>>(HttpStatusCode.OK, campaigns);
    }

    public async Task<Response> UpdateAsync(int id, CampaignUpdateDto payload)
    {
        var campaign = await dbContext.Campaigns.FindAsync(id);

        if (campaign is null)
        {
            return new Response(HttpStatusCode.NotFound);
        }

        await dbContext.Entry(campaign).Collection(c => c.Players).LoadAsync();

        var players = campaign.Players.Where(u => payload.PlayerIds.Contains(u.Id));

        campaign.Players = players.ToList();
        campaign.Name = payload.Name;

        await dbContext.SaveChangesAsync();

        return new Response(HttpStatusCode.OK);
    }
}