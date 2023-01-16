using DataTransfer;
using DataTransfer.CampaignCreation;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CampaignCreationController : ControllerBase
    {
        private readonly ICampaignCreation _campaignCreation;

        public CampaignCreationController(ICampaignCreation campaignCreation)
        {
            _campaignCreation = campaignCreation;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<CampaignCreationDto>>> GetAsync(int campaignId)
        {
            var response = await _campaignCreation.GetAsync(campaignId);
            return this.SendResponse<CampaignCreationDto>(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> PostAsync(CampaignCreationDto payload)
        {
            var response = await _campaignCreation.PostAsync(payload);
            // TODO: Create entry in 'ActiveCampaignElements' initialize with '-1'
            return this.SendResponse(response);
        }
    }
}