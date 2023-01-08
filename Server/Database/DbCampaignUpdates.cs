namespace Server.Database
{
    public class DbCampaignUpdates
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public int MapChange { get; set; }
        public int TokenChange { get; set; }
        public int DiceRoll { get; set; }
        public int MusicChange { get; set; }
    }
}