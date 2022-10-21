using DataTransfer;
using DataTransfer.CampaignSelection;
using DataTransfer.User;
using Microsoft.EntityFrameworkCore;

namespace Server.Services
{
    public interface ICampaignOverview
    {
        public Task<ApiResponse<CampaignOverviewDto>> GetAsync(int userId);
    }

    public class CampaignOverview : ICampaignOverview
    {
        private readonly SQLDatabase _dbContext;

        public CampaignOverview(SQLDatabase dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApiResponse<CampaignOverviewDto>> GetAsync(int userId)
        {
            try
            {
                var campainsWithUser = _dbContext.Campaigns.
                    Where(x => _dbContext.UsersInCampaign.
                    Where(x => x.UserId == userId).
                    Any(y => y.CampaignId == x.Id));

                var campaignOverviewItems = new List<CampaignOverviewItemDto>();

                foreach (var campaign in campainsWithUser)
                {
                    var usersInCampaignWithUser = _dbContext.UsersInCampaign.Where(x => x.CampaignId == campaign.Id);
                    var gamemasterInCampaignWithUser = await usersInCampaignWithUser.FirstAsync(x => x.IsGamemaster);
                    var playersInCampaignWithUser = usersInCampaignWithUser.Where(x => !x.IsGamemaster);

                    var gamemaster = await _dbContext.Users.FirstAsync(x => gamemasterInCampaignWithUser.UserId == x.Id);
                    var players = _dbContext.Users.Where(x => playersInCampaignWithUser.Any(y => x.Id == y.UserId)).Select(x => x.Username);

                    campaignOverviewItems.Add(new()
                    {
                        Id = campaign.Id,
                        Name = campaign.Name,
                        Gamemaster = gamemaster.Username,
                        IsGamemaster = gamemaster.Id == userId,
                        Characters = await players.ToListAsync()
                    });
                }

                CampaignOverviewDto payload = new() { CampaignItems = campaignOverviewItems };

                return ApiResponse<CampaignOverviewDto>.Success(payload);
            }
            catch (Exception exception)
            {
                return ApiResponse<CampaignOverviewDto>.Failure(new ErrorDetails(ErrorCode.Exception, exception.Message));
            }
        }
    }
}