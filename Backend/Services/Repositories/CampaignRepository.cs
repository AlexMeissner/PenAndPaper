using System.Net;
using Backend.Database;
using Backend.Database.Models;
using DataTransfer.Campaign;
using DataTransfer.Response;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services.Repositories;

public interface ICampaignRepository
{
    Task<Response<int>> CreateAsync(IdentityClaims identity, CampaignCreationDto payload);
    Task<Response<CampaignDto>> GetAsync(IdentityClaims identity, int id);
    Response<IEnumerable<CampaignsDto>> GetAll(IdentityClaims identity);
    Task<Response> UpdateAsync(int id, CampaignUpdateDto payload);
}

public class CampaignRepository(PenAndPaperDatabase dbContext) : ICampaignRepository
{
    public async Task<Response<int>> CreateAsync(IdentityClaims identity, CampaignCreationDto payload)
    {
        var players = await dbContext.Users.Where(u => payload.PlayerIds.Contains(u.Id)).ToListAsync();

        var campaign = new Campaign()
        {
            Name = payload.Name,
            GameMaster = identity.User,
            Players = players
        };

        await dbContext.AddAsync(campaign);
        await dbContext.SaveChangesAsync();

        return new Response<int>(HttpStatusCode.Created, campaign.Id);
    }

    public async Task<Response<CampaignDto>> GetAsync(IdentityClaims identity, int id)
    {
        var campaign = await dbContext.Campaigns
            .Include(c => c.GameMaster)
            .Include(c => c.Players)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (campaign is null)
        {
            return new Response<CampaignDto>(HttpStatusCode.NotFound);
        }

        var players = campaign.Players.Select(p => new CampaignUser(p.Id, p.Username)).ToList();

        var playerIds = players.Select(p => p.Id);
        var uninvitedUsers = dbContext.Users
            .Where(u => !playerIds.Contains(u.Id) && u != identity.User)
            .Select(u => new CampaignUser(u.Id, u.Username));

        var isGameMaster = campaign.GameMaster == identity.User;
        var payload = new CampaignDto(campaign.Name, players, uninvitedUsers, isGameMaster);

        return new Response<CampaignDto>(HttpStatusCode.OK, payload);
    }

    public Response<IEnumerable<CampaignsDto>> GetAll(IdentityClaims identity)
    {
        var campaigns = dbContext.Campaigns
            .Include(c => c.GameMaster)
            .Include(c => c.Players)
            .Where(c => c.GameMaster == identity.User || c.Players.Contains(identity.User))
            .Select(c => new CampaignsDto(c.Id, c.Name, c.GameMaster.Username, c.Players.Select(u => u.Username),
                c.GameMaster == identity.User));

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