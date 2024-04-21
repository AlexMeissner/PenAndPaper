using DataTransfer.Campaign;
using Microsoft.AspNetCore.Mvc;
using Server.Services.BusinessLogic;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CampaignCreationController(ICampaignManager campaignManager) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(int campaignId, int userId)
        {
            var creationData = await campaignManager.GetCreationDataAsync(campaignId, userId);

            if (creationData is null)
            {
                return NotFound(campaignId);
            }

            return Ok(creationData);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CampaignCreationDto payload)
        {
            var campaignId = await campaignManager.Create(payload);
            return CreatedAtAction(nameof(Get), campaignId);
        }

        [HttpPut]
        public async Task<IActionResult> Put(CampaignCreationDto payload)
        {
            var updated = await campaignManager.Update(payload);

            if (updated is null)
            {
                return NotFound(payload);
            }
            else if (updated is false)
            {
                return this.InternalServerError(payload);
            }

            return Ok(payload);
        }
    }
}
