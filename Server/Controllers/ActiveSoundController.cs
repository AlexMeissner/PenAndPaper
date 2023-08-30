using DataTransfer.Sound;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Database;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActiveSoundController : ControllerBase
    {
        private readonly SQLDatabase _dbContext;

        public ActiveSoundController(SQLDatabase dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int campaignId)
        {
            try
            {
                var activeCampaignElements = await _dbContext.ActiveCampaignElements.FirstAsync(x => x.CampaignId == campaignId);

                var payload = new ActiveSoundDto(activeCampaignElements.CampaignId, activeCampaignElements.AmbientId, activeCampaignElements.EffectId);

                return Ok(payload);
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(ActiveSoundDto payload)
        {
            try
            {
                var activeCampaignElements = await _dbContext.ActiveCampaignElements.FirstAsync(x => x.CampaignId == payload.CampaignId);
                var update = await _dbContext.CampaignUpdates.FirstAsync(x => x.CampaignId == payload.CampaignId);

                if (payload.AmbientId is int ambientId && activeCampaignElements.AmbientId != ambientId)
                {
                    update.AmbientSoundChange = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    activeCampaignElements.AmbientId = ambientId;
                    await _dbContext.SaveChangesAsync();
                }
                if (payload.EffectId is int effectId && activeCampaignElements.EffectId != effectId)
                {
                    update.SoundEffectChange = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    activeCampaignElements.EffectId = effectId;
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