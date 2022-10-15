namespace DataTransfer.CampaignSelection
{
    public class CampaignSelectionItem
    {
        public string? Name { get; }
        public string? Gamemaster { get; }
        public ICollection<string>? Characters { get; }
    }

    public class CampaignSelectionDto
    {
        public ICollection<CampaignSelectionItem>? CampaignItems { get; }
    }
}