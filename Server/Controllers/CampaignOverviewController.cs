using DataTransfer;
using DataTransfer.CampaignSelection;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CampaignOverviewController : ControllerBase
    {
        private readonly ICampaignOverview _campaignOverview;

        public CampaignOverviewController(ICampaignOverview campaignOverview)
        {
            _campaignOverview = campaignOverview;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<CampaignOverviewDto>>> GetAsync(int userId)
        {
            var response = await _campaignOverview.GetAsync(userId);
            return this.SendResponse<CampaignOverviewDto>(response);
        }
    }
}