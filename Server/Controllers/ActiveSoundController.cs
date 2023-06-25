using DataTransfer;
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
        private readonly ILogger<ActiveSoundController> _logger;

        public ActiveSoundController(SQLDatabase dbContext, ILogger<ActiveSoundController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<ActiveSoundDto>>> GetAsync(int campaignId)
        {
            _logger.LogInformation(nameof(GetAsync));

            ActiveSoundDto payload;

            try
            {
                var activeCampaignElements = await _dbContext.ActiveCampaignElements.FirstAsync(x => x.CampaignId == campaignId);

                payload = new()
                {
                    CampaignId = activeCampaignElements.CampaignId,
                    AmbientId = activeCampaignElements.AmbientId,
                    EffectId = activeCampaignElements.EffectId,
                };
            }
            catch (Exception exception)
            {
                return ApiResponse<ActiveSoundDto>.Failure(new ErrorDetails(ErrorCode.Exception, exception.Message));
            }

            return ApiResponse<ActiveSoundDto>.Success(payload);
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse>> PutAsync(ActiveSoundDto payload)
        {
            _logger.LogInformation(nameof(PutAsync));

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

                return ApiResponse.Success;
            }
            catch (Exception exception)
            {
                return ApiResponse.Failure(new ErrorDetails(ErrorCode.Exception, exception.Message));
            }
        }
    }
}