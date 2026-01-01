using Microsoft.EntityFrameworkCore;
using Website.Database;
using Website.Events;

namespace Website.Services.Repositories;

public record ChatUser(int? UserId, string Name);

public interface IChatRepository
{
    Task<List<ChatUser>> GetUsers(int campaignId);
    Task SendMessage(int campaignId, int? targetUserId, string message);
}

public class ChatRepository(IDbContextFactory<PenAndPaperDatabase> dbContextFactory, ICampaignEventHub campaignEventHub, IUserClaims claims) : IChatRepository
{
    public async Task<List<ChatUser>> GetUsers(int campaignId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var campaign = await dbContext.Campaigns
            .AsNoTracking()
            .Include(c => c.GameMaster)
            .Include(c => c.Players)
            .FirstOrDefaultAsync(c => c.Id == campaignId);

        if (campaign is null) return [];

        var chatUsers = campaign.Players
            .Append(campaign.GameMaster)
            .Where(u => u.Id != claims.UserId)
            .Select(u => new ChatUser(u.Id, u.Username))
            .OrderBy(u => u.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();

        chatUsers.Insert(0, new ChatUser(null, "Alle"));

        return chatUsers;
    }

    public async Task SendMessage(int campaignId, int? targetUserId, string message)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var user = await claims.GetUserAsync();

        var image = await GetAvatar(user.Id, campaignId);

        var receiverEvent = new ChatMessageEvent
        (
            DateTime.UtcNow,
            ChatMessageType.Message,
            user.Id,
            user.Username,
            message,
            image,
            targetUserId != null
        );

        campaignEventHub.ForCampaign(campaignId).Publish(receiverEvent);
    }

    private async Task<string?> GetAvatar(int userId, int campaignId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var campaign = await dbContext.Campaigns.FindAsync(campaignId);

        if (campaign is not null && campaign.GameMasterId == userId)
        {
            return DungeonMasterIcon;
        }

        var character = await dbContext.Characters
            .AsNoTracking()
            .OrderBy(c => c.Id)
            .LastOrDefaultAsync(c => c.UserId == userId && c.CampaignId == campaignId);

        return character is null ? null : $"data:image/png;base64,{Convert.ToBase64String(character.Image)}";
    }

    // ToDo: Generate new image - shortened constant to reduce file size
    private const string DungeonMasterIcon = "data:image/jpeg;base64,/9j/4RBASE64...";
}
