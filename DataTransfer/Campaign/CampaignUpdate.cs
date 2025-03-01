namespace DataTransfer.Campaign;

public record CampaignUpdateDto(string Name, IEnumerable<int> PlayerIds);