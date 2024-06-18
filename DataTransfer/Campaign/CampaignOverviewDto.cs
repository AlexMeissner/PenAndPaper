namespace DataTransfer.Campaign
{
    // ToDo: Characters should be called Players, right?
    public record CampaignOverviewItemDto(int Id, string Name, string Gamemaster, bool IsGamemaster, ICollection<string> Characters);

    public record CampaignOverviewDto(ICollection<CampaignOverviewItemDto> CampaignItems);
}