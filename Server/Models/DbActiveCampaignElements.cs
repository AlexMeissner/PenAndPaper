namespace Server.Models
{
    public class DbActiveCampaignElements
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public int MapId { get; set; }
        public int AmbientId { get; set; }
        public int EffectId { get; set; }
    }
}