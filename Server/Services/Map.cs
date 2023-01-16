using DataTransfer;
using DataTransfer.Map;
using Microsoft.EntityFrameworkCore;
using Server.Database;

namespace Server.Services
{
    public interface IMap
    {
        public Task<ApiResponse<MapDto>> GetAsync(int mapId);
        public Task<ApiResponse> PostAsync(MapDto payload);
        // TODO: Update
    }

    public class Map : IMap
    {
        private readonly SQLDatabase _dbContext;

        public Map(SQLDatabase dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApiResponse<MapDto>> GetAsync(int mapId)
        {
            MapDto payload;

            try
            {
                var map = await _dbContext.Maps.FirstAsync(x => x.Id == mapId);

                payload = new()
                {
                    Id = map.Id,
                    CampaignId = map.CampaignId,
                    Name = map.Name,
                    ImageData = map.ImageData,
                    Grid = new()
                    {
                        IsActive = map.GridIsActive,
                        Size = map.GridSize
                    }
                };
            }
            catch (Exception exception)
            {
                return ApiResponse<MapDto>.Failure(new ErrorDetails(ErrorCode.Exception, exception.Message));
            }

            return ApiResponse<MapDto>.Success(payload);
        }

        public async Task<ApiResponse> PostAsync(MapDto payload)
        {
            try
            {
                var dbMap = new DbMap()
                {
                    CampaignId = payload.CampaignId,
                    Name = payload.Name,
                    ImageData = payload.ImageData,
                    GridIsActive = payload.Grid.IsActive,
                    GridSize = payload.Grid.Size,
                };

                await _dbContext.Maps.AddAsync(dbMap);
                await _dbContext.SaveChangesAsync();

                return ApiResponse.Success;
            }
            catch (Exception exception)
            {
                return ApiResponse.Failure(new ErrorDetails(ErrorCode.Exception, exception.Message));
            }
        }
    }
}