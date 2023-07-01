namespace Server.Database
{
    public class DbCharactersInCampaign
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public int CharacterId { get; set; }
        public int UserId { get; set; }
    }
}
