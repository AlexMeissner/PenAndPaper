using DataTransfer;
using DataTransfer.Campaign;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CampaignUpdatesController : ControllerBase
    {
        private readonly ICampaignUpdates _campaignUpdates;

        public CampaignUpdatesController(ICampaignUpdates campaignUpdates)
        {
            _campaignUpdates = campaignUpdates;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<CampaignUpdateDto>>> GetAsync(int campaignId)
        {
            var response = await _campaignUpdates.GetAsync(campaignId);
            return this.SendResponse<CampaignUpdateDto>(response);
        }
    }
}