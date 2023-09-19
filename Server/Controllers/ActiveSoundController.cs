using DataTransfer.Sound;
using DataTransfer.WebSocket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActiveSoundController : ControllerBase
    {
        private readonly SQLDatabase _dbContext;
        private readonly IUpdateNotifier _updateNotifier;

        public ActiveSoundController(SQLDatabase dbContext, IUpdateNotifier updateNotifier)
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

                if (payload.AmbientId is int ambientId && activeCampaignElements.AmbientId != ambientId)
                {
                    activeCampaignElements.AmbientId = ambientId;
                    await _dbContext.SaveChangesAsync();
                    await _updateNotifier.Send(payload.CampaignId, UpdateEntity.AmbientSound);
                }
                if (payload.EffectId is int effectId && activeCampaignElements.EffectId != effectId)
                {
                    activeCampaignElements.EffectId = effectId;
                    await _dbContext.SaveChangesAsync();
                    await _updateNotifier.Send(payload.CampaignId, UpdateEntity.SoundEffect);
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