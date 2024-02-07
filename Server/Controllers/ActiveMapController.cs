using DataTransfer.Map;
using Microsoft.AspNetCore.Mvc;
using Server.Services.BusinessLogic;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActiveMapController(ICampaignManager campaignManager) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(int campaignId)
        {
            try
            {
                var activeMap = await campaignManager.GetActiveCampaignElements(campaignId);

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
                var updated = await campaignManager.UpdateActiveCampaignElements(payload);

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