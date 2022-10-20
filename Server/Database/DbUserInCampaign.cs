namespace Server.Database
{
    public class DbUserInCampaign
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CampaignId { get; set; }
        public bool IsGamemaster { get; set; }
    }
}