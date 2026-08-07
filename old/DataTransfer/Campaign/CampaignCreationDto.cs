using DataTransfer.User;

namespace DataTransfer.Campaign;

public record CampaignCreationDto(string Name, IEnumerable<int> PlayerIds);