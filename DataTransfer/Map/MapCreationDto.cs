namespace DataTransfer.Map
{
    public class GridDto
    {
        public bool IsActive { get; set; }
        public int Size { get; set; }
    }

    public class MapCreationDto : PropertyChangedNotifier
    {
        public byte[]? ImageData { get; set; }
        public GridDto Grid { get; set; } = new();
    }
}