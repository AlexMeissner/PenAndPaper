namespace DataTransfer.Map
{
    public class MapOverviewItemDto
    {
        public string Name { get; set; } = string.Empty;
        public int MapId { get; set; }
        public byte[]? ImageData { get; set; }
    }

    public class MapOverviewDto : PropertyChangedNotifier
    {
        public ICollection<MapOverviewItemDto> Items { get; set; } = Array.Empty<MapOverviewItemDto>();
    }
}