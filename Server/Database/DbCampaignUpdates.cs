namespace Server.Database
{
    public class DbCampaignUpdates
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public long MapChange { get; set; }
        public long MapCollectionChange { get; set; }
        public long TokenChange { get; set; }
        public long DiceRoll { get; set; }
        public long SoundChange { get; set; }
    }
}