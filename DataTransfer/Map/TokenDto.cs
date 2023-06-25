namespace DataTransfer.Map
{
    public class TokenDto : IMapItem
    {
        public MapItemType Type => MapItemType.Token;
        public int X { get; set; }
        public int Y { get; set; }
        public int Id { get; set; } // Either player or npc id
        public string Name { get; set; }
        public byte[] Image { get; set; }
    }
}
