namespace DataTransfer.Map
{
    public class MapOverviewItemDto : PropertyChangedNotifier
    {
        public string Name { get; set; } = string.Empty;
        public byte[]? ImageData { get; set; }
    }

    public class MapOverviewDto
    {
        public ICollection<MapOverviewItemDto> Items { get; set; }
    }
}