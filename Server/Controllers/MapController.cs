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
        private readonly ILogger<MapController> _logger;

        public MapController(SQLDatabase dbContext, ILogger<MapController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<MapDto>>> GetAsync(int mapId)
        {
            _logger.LogInformation(nameof(GetAsync));

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
            _logger.LogInformation(nameof(PostAsync));

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
            _logger.LogInformation(nameof(PutAsync));

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
            _logger.LogInformation(nameof(DeleteAsync));

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