namespace DataTransfer.Campaign;

public record CampaignUser(int Id, string Name);

public record CampaignDto(
    string Name,
    IEnumerable<CampaignUser> Players,
    IEnumerable<CampaignUser> UninvitedUsers,
    bool IsGameMaster);