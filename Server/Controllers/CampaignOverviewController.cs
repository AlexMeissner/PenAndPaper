using DataTransfer.Campaign;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CampaignOverviewController(SQLDatabase dbContext) : ControllerBase
    {
        [HttpGet]
        public IActionResult Get(int userId)
        {
            var campainsWithUser = dbContext.Campaigns
                .Include(c => c.Players)
                .Include(c => c.Gamemaster)
                .Where(c => c.Players.Any(u => u.Id == userId) || c.Gamemaster.Id == userId);

            var campaignOverviewItems = new List<CampaignOverviewItemDto>();

            foreach (var campaign in campainsWithUser)
            {
                var players = campaign.Players.Select(u => u.Username).ToList();

                campaignOverviewItems.Add(new CampaignOverviewItemDto(
                    Id: campaign.Id,
                    Name: campaign.Name,
                    Gamemaster: campaign.Gamemaster.Username,
                    IsGamemaster: campaign.Gamemaster.Id == userId,
                    Characters: players));
            }

            var payload = new CampaignOverviewDto(campaignOverviewItems);

            return Ok(payload);
        }
    }
}
