using DataTransfer.Campaign;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CampaignOverviewController : ControllerBase
    {
        private readonly SQLDatabase _dbContext;

        public CampaignOverviewController(SQLDatabase dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int userId)
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

                    campaignOverviewItems.Add(new CampaignOverviewItemDto(
                        Id: campaign.Id,
                        Name: campaign.Name,
                        Gamemaster: gamemaster.Username,
                        IsGamemaster: gamemaster.Id == userId,
                        Characters: await players.ToListAsync()));
                }

                var payload = new CampaignOverviewDto(campaignOverviewItems);

                return Ok(payload);
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }
        }
    }
}