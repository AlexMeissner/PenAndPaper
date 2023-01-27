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

        public CampaignUpdatesController(SQLDatabase dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<CampaignUpdateDto>>> GetAsync(int campaignId)
        {
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
                    MusicChange = campaignUpdate.MusicChange
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