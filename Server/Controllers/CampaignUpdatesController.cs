using DataTransfer;
using DataTransfer.Campaign;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Database;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CampaignUpdatesController : ControllerBase
    {
        private readonly SQLDatabase _dbContext;
        private readonly ILogger<CampaignUpdatesController> _logger;

        public CampaignUpdatesController(SQLDatabase dbContext, ILogger<CampaignUpdatesController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<CampaignUpdateDto>>> GetAsync(int campaignId)
        {
            _logger.LogInformation(nameof(GetAsync));

            try
            {
                var campaignUpdate = await _dbContext.CampaignUpdates.FirstAsync(x => x.CampaignId == campaignId);

                var payload = new CampaignUpdateDto()
                {
                    CampaignId = campaignUpdate.CampaignId,
                    MapChange = campaignUpdate.MapChange,
                    MapCollectionChange = campaignUpdate.MapCollectionChange,
                    TokenChange = campaignUpdate.TokenChange,
                    DiceRoll = campaignUpdate.DiceRoll,
                    AmbientSoundChange = campaignUpdate.AmbientSoundChange,
                    SoundEffectChange = campaignUpdate.SoundEffectChange
                };

                var response = ApiResponse<CampaignUpdateDto>.Success(payload);
                return this.SendResponse<CampaignUpdateDto>(response);
            }
            catch (Exception exception)
            {
                var response = ApiResponse<CampaignUpdateDto>.Failure(new ErrorDetails(ErrorCode.Exception, exception.Message));
                return this.SendResponse<CampaignUpdateDto>(response);
            }
        }
    }
}