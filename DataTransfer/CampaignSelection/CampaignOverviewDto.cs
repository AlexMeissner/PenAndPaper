namespace DataTransfer.CampaignSelection
{
    public class CampaignOverviewItemDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Gamemaster { get; set; }
        public bool IsGamemaster { get; set; }
        public ICollection<string>? Characters { get; set; }
    }

    public class CampaignOverviewDto
    {
        public ICollection<CampaignOverviewItemDto>? CampaignItems { get; set; }
    }
}