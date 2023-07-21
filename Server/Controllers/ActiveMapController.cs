using DataTransfer.Map;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Database;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActiveMapController : ControllerBase
    {
        private readonly SQLDatabase _dbContext;

        public ActiveMapController(SQLDatabase dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(int campaignId)
        {
            try
            {
                var activeCampaignElements = await _dbContext.ActiveCampaignElements.FirstAsync(x => x.CampaignId == campaignId);

                var payload = new ActiveMapDto()
                {
                    CampaignId = activeCampaignElements.CampaignId,
                    MapId = activeCampaignElements.MapId,
                };

                return Ok(payload);
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }

        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(ActiveMapDto payload)
        {
            try
            {
                var activeCampaignElements = await _dbContext.ActiveCampaignElements.FirstAsync(x => x.CampaignId == payload.CampaignId);
                var update = await _dbContext.CampaignUpdates.FirstAsync(x => x.CampaignId == payload.CampaignId);

                if (activeCampaignElements.MapId != payload.MapId)
                {
                    update.MapChange = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    activeCampaignElements.MapId = payload.MapId;
                    await _dbContext.SaveChangesAsync();
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