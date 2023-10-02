using DataTransfer.Map;
using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Server.Services.BusinessLogic;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActiveMapController : ControllerBase
    {
        private readonly ICampaign _campaign;
        private readonly IUpdateNotifier _updateNotifier;

        public ActiveMapController(ICampaign campaign, IUpdateNotifier updateNotifier)
        {
            _campaign = campaign;
            _updateNotifier = updateNotifier;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int campaignId)
        {
            try
            {
                var activeMap = await _campaign.GetActiveCampaignElements(campaignId);

                if (activeMap is null)
                {
                    return NotFound(campaignId);
                }

                return Ok(activeMap);
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(ActiveMapDto payload)
        {
            try
            {
                var updated = await _campaign.UpdateActiveCampaignElements(payload);

                if (updated is null)
                {
                    return NotFound(payload);
                }

                if (updated is false)
                {
                    return this.NotModified(payload);
                }

                return Ok(payload);
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }
        }
    }
}