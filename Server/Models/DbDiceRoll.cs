namespace Server.Models
{
    public class DbDiceRoll
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public string Roll { get; set; } = default!;
    }
}