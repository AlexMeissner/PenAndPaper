namespace Server.Database
{
    public class DbMap
    {
        public int Id { get; set; }
        public int CampaignId { get; set; } // TODO: Design Flaw: there should be another table id|campaign id|map id
        public string Name { get; set; }
        public byte[]? ImageData { get; set; }
        public bool GridIsActive { get; set; }
        public int GridSize { get; set; }
    }
}