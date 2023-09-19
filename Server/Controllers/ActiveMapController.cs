using DataTransfer.Map;
using DataTransfer.WebSocket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActiveMapController : ControllerBase
    {
        private readonly SQLDatabase _dbContext;
        private readonly IUpdateNotifier _updateNotifier;

        public ActiveMapController(SQLDatabase dbContext, IUpdateNotifier updateNotifier)
        {
            _dbContext = dbContext;
            _updateNotifier = updateNotifier;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int campaignId)
        {
            try
            {
                var activeCampaignElements = await _dbContext.ActiveCampaignElements.FirstAsync(x => x.CampaignId == campaignId);

                var payload = new ActiveMapDto(activeCampaignElements.CampaignId, activeCampaignElements.MapId);

                return Ok(payload);
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
                var activeCampaignElements = await _dbContext.ActiveCampaignElements.FirstAsync(x => x.CampaignId == payload.CampaignId);

                if (activeCampaignElements.MapId != payload.MapId)
                {
                    activeCampaignElements.MapId = payload.MapId;
                    await _dbContext.SaveChangesAsync();
                    await _updateNotifier.Send(payload.CampaignId, UpdateEntity.Map);
                }

                return Ok(activeCampaignElements);
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }
        }
    }
}