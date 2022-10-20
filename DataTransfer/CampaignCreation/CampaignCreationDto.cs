using DataTransfer.User;

namespace DataTransfer.CampaignCreation
{
    public class CampaignCreationDto
    {
        public int CampaignId { get; set; }
        public string CampaignName { get; set; }
        public UsersDto? Gamemaster { get; set; }
        public ICollection<UsersDto> UsersNotInCampaign { get; set; }
        public ICollection<UsersDto> UsersInCampaign { get; set; }
    }
}