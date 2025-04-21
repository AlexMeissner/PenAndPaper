using System.Net;
using Backend.Database;
using DataTransfer.Campaign;
using DataTransfer.Chat;
using DataTransfer.Response;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services.Repositories;

public interface IUserRepository
{
    Task<bool> ExistsAsync(int userId);
    Task<Response<IEnumerable<ChatUserDto>>> GetChatUsers(IdentityClaims identity, int campaignId);
    Response<IEnumerable<CampaignUser>> GetAll(IdentityClaims identity);
    Task<string?> GetAvatar(int userId, int campaignId);
}

public class UserRepository(PenAndPaperDatabase dbContext) : IUserRepository
{
    public async Task<bool> ExistsAsync(int userId)
    {
        return await dbContext.Users.FindAsync(userId) != null;
    }

    public async Task<Response<IEnumerable<ChatUserDto>>> GetChatUsers(IdentityClaims identity, int campaignId)
    {
        var campaign = await dbContext.Campaigns
            .Include(c => c.GameMaster)
            .Include(c => c.Players)
            .FirstOrDefaultAsync(c => c.Id == campaignId);

        if (campaign is null) return new Response<IEnumerable<ChatUserDto>>(HttpStatusCode.NotFound);

        var chatUsers = campaign.Players
            .Append(campaign.GameMaster)
            .Where(u => u != identity.User)
            .Select(u => new ChatUserDto(u.Id, u.Username))
            .OrderBy(u => u.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();

        chatUsers.Insert(0, new ChatUserDto(null, "Alle"));

        return new Response<IEnumerable<ChatUserDto>>(HttpStatusCode.OK, chatUsers);
    }

    public Response<IEnumerable<CampaignUser>> GetAll(IdentityClaims identity)
    {
        var users = dbContext.Users
            .Where(u => u != identity.User)
            .Select(u => new CampaignUser(u.Id, u.Username));

        return new Response<IEnumerable<CampaignUser>>(HttpStatusCode.OK, users);
    }

    public async Task<string?> GetAvatar(int userId, int campaignId)
    {
        var character = await dbContext.Characters
            .OrderBy(c => c.Id)
            .LastOrDefaultAsync(c => c.UserId == userId && c.CampaignId == campaignId);

        return character is null ? null : $"data:image/png;base64,{Convert.ToBase64String(character.Image)}";
    }
}