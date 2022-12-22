namespace DataTransfer.Map
{
    public class MapOverviewDto : PropertyChangedNotifier
    {
        public string Name { get; set; } = string.Empty;
        public byte[]? ImageData { get; set; }
    }
}