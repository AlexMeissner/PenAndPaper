using DataTransfer;
using DataTransfer.Map;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Database;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MapController : ControllerBase
    {
        private readonly SQLDatabase _dbContext;

        public MapController(SQLDatabase dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<MapDto>>> GetAsync(int mapId)
        {
            try
            {
                var map = await _dbContext.Maps.FirstAsync(x => x.Id == mapId);

                var payload = new MapDto()
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

                return this.SendResponse<MapDto>(ApiResponse<MapDto>.Success(payload));
            }
            catch (Exception exception)
            {
                return this.SendResponse<MapDto>(ApiResponse<MapDto>.Failure(new ErrorDetails(ErrorCode.Exception, exception.Message)));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> PostAsync(MapDto payload)
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

                var update = await _dbContext.CampaignUpdates.FirstAsync(x => x.CampaignId == payload.CampaignId);
                update.MapCollectionChange = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                await _dbContext.SaveChangesAsync();

                return ApiResponse.Success;
            }
            catch (Exception exception)
            {
                return ApiResponse.Failure(new ErrorDetails(ErrorCode.Exception, exception.Message));
            }
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse>> PutAsync(MapDto payload)
        {
            try
            {
                var map = await _dbContext.Maps.FirstAsync(x => x.Id == payload.Id);

                map.Name = payload.Name;
                map.ImageData = payload.ImageData;
                map.GridIsActive = payload.Grid.IsActive;
                map.GridSize = payload.Grid.Size;

                var update = await _dbContext.CampaignUpdates.FirstAsync(x => x.CampaignId == payload.CampaignId);
                update.MapCollectionChange = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                await _dbContext.SaveChangesAsync();

                return ApiResponse.Success;
            }
            catch (Exception exception)
            {
                return ApiResponse.Failure(new ErrorDetails(ErrorCode.Exception, exception.Message));
            }
        }

        [HttpDelete]
        public async Task<ActionResult<ApiResponse>> DeleteAsync(int mapId)
        {
            try
            {
                var map = await _dbContext.Maps.FirstAsync(x => x.Id == mapId);

                var update = await _dbContext.CampaignUpdates.FirstAsync(x => x.CampaignId == map.CampaignId);
                update.MapCollectionChange = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                _dbContext.Maps.Remove(map);

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