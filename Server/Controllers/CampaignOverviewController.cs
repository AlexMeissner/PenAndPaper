using DataTransfer;
using DataTransfer.CampaignSelection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Database;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CampaignOverviewController : ControllerBase
    {
        private readonly SQLDatabase _dbContext;
        private readonly ILogger<CampaignOverviewController> _logger;

        public CampaignOverviewController(SQLDatabase dbContext, ILogger<CampaignOverviewController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<CampaignOverviewDto>>> GetAsync(int userId)
        {
            _logger.LogInformation(nameof(GetAsync));

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
                var response = ApiResponse<CampaignOverviewDto>.Success(payload);

                return this.SendResponse<CampaignOverviewDto>(response);
            }
            catch (Exception exception)
            {
                var response = ApiResponse<CampaignOverviewDto>.Failure(new ErrorDetails(ErrorCode.Exception, exception.Message));
                return this.SendResponse<CampaignOverviewDto>(response);
            }
        }
    }
}