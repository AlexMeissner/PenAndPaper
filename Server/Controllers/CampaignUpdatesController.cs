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

        public CampaignUpdatesController(SQLDatabase dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int campaignId)
        {
            try
            {
                var campaignUpdate = await _dbContext.CampaignUpdates.FirstAsync(x => x.CampaignId == campaignId);

                var payload = new CampaignUpdateDto(
                    CampaignId: campaignUpdate.CampaignId,
                    MapChange: campaignUpdate.MapChange,
                    MapCollectionChange: campaignUpdate.MapCollectionChange,
                    TokenChange: campaignUpdate.TokenChange,
                    DiceRoll: campaignUpdate.DiceRoll,
                    AmbientSoundChange: campaignUpdate.AmbientSoundChange,
                    SoundEffectChange: campaignUpdate.SoundEffectChange
                );

                return Ok(payload);
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }
        }
    }
}