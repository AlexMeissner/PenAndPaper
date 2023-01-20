using DataTransfer;
using DataTransfer.Map;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Services;

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
        public async Task<ActionResult<ApiResponse<ActiveMapDto>>> GetAsync(int campaignId)
        {
            ActiveMapDto payload;

            try
            {
                var activeCampaignElements = await _dbContext.ActiveCampaignElements.FirstAsync(x => x.CampaignId == campaignId);

                payload = new()
                {
                    CampaignId = activeCampaignElements.CampaignId,
                    MapId = activeCampaignElements.MapId,
                };
            }
            catch (Exception exception)
            {
                return ApiResponse<ActiveMapDto>.Failure(new ErrorDetails(ErrorCode.Exception, exception.Message));
            }

            return ApiResponse<ActiveMapDto>.Success(payload);
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse>> PutAsync(ActiveMapDto payload)
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

                return ApiResponse.Success;
            }
            catch (Exception exception)
            {
                return ApiResponse.Failure(new ErrorDetails(ErrorCode.Exception, exception.Message));
            }
        }
    }
}