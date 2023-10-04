using DataTransfer.Campaign;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services.BusinessLogic;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CampaignCreationController : ControllerBase
    {
        private readonly SQLDatabase _dbContext;
        private readonly ICampaign _campaign;

        public CampaignCreationController(SQLDatabase dbContext, ICampaign campaign)
        {
            _dbContext = dbContext;
            _campaign = campaign;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int campaignId, int userId)
        {
            try
            {
                var creationData = await _campaign.GetCreationDataAsync(campaignId, userId);

                if (creationData is null)
                {
                    return NotFound(campaignId);
                }

                return Ok(creationData);
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(CampaignCreationDto payload)
        {
            try
            {
                var campaignId = await _campaign.Create(payload);
                return CreatedAtAction(nameof(Get), campaignId);
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }
        }
    }
}