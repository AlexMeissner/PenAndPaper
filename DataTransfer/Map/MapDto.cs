namespace DataTransfer.Map
{
    public class GridDto : PropertyChangedNotifier
    {
        public bool IsActive { get; set; }
        public int Size { get; set; }
    }

    public class MapDto : PropertyChangedNotifier
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte[]? ImageData { get; set; }
        public GridDto Grid { get; set; } = new();
    }
}