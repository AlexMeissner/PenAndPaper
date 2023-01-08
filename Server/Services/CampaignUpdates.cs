using DataTransfer;
using DataTransfer.Campaign;
using Microsoft.EntityFrameworkCore;

namespace Server.Services
{
    public interface ICampaignUpdates
    {
        public Task<ApiResponse<CampaignUpdateDto>> GetAsync(int campaignId);
    }

    public class CampaignUpdates : ICampaignUpdates
    {
        private readonly SQLDatabase _dbContext;

        public CampaignUpdates(SQLDatabase dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApiResponse<CampaignUpdateDto>> GetAsync(int campaignId)
        {
            CampaignUpdateDto payload;

            try
            {
                var campaignUpdate = await _dbContext.CampaignUpdates.FirstAsync(x => x.CampaignId == campaignId);



                payload = new()
                {
                    CampaignId = campaignUpdate.CampaignId,
                    MapChange = campaignUpdate.MapChange,
                    TokenChange = campaignUpdate.TokenChange,
                    DiceRoll = campaignUpdate.DiceRoll,
                    MusicChange = campaignUpdate.MusicChange
                };
            }
            catch (Exception exception)
            {
                return ApiResponse<CampaignUpdateDto>.Failure(new ErrorDetails(ErrorCode.Exception, exception.Message));
            }

            return ApiResponse<CampaignUpdateDto>.Success(payload);
        }
    }
}