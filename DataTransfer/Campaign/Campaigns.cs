namespace DataTransfer.Campaign;

public record CampaignsDto(int Id, string Name, string GameMaster, IEnumerable<string> Players, bool IsGameMaster);