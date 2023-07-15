namespace Server.Database
{
    public class DbToken
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int? CharacterId { get; set; }
        public int? MonsterId { get; set; }
    }
}
