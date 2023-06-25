namespace DataTransfer.Map
{
    public enum MapItemType
    {
        Token,
        Location,
        Landmark
    }

    public interface IMapItem
    {
        public MapItemType Type { get; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
