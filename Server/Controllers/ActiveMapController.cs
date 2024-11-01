using DataTransfer.Map;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Server.Hubs;
using Server.Services.BusinessLogic;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActiveMapController(
        ICampaignManager campaignManager,
        IHubContext<CampaignUpdateHub, ICampaignUpdate> campaignUpdateHub) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(int campaignId)
        {
            var activeMap = await campaignManager.GetActiveCampaignElements(campaignId);

            if (activeMap is null)
            {
                return NotFound(campaignId);
            }

            return Ok(activeMap);
        }

        [HttpPut]
        public async Task<IActionResult> Put(ActiveMapDto payload)
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

            // ToDo: payload.MapId should not be null
            var eventArgs = new MapChangedEventArgs((int)payload.MapId);
            await campaignUpdateHub.Clients.All.MapChanged(eventArgs);

            return Ok(payload);
        }
    }
}