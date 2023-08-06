using DataTransfer.User;

namespace DataTransfer.Campaign
{
    public record CampaignCreationDto(
        int CampaignId,
        string CampaignName,
        UsersDto Gamemaster,
        ICollection<UsersDto> UsersNotInCampaign,
        ICollection<UsersDto> UsersInCampaign);
}
