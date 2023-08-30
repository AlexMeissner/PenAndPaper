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
        public async Task<IActionResult> Get(int mapId)
        {
            try
            {
                var map = await _dbContext.Maps.FirstAsync(x => x.Id == mapId);

                var payload = new MapDto(
                    Id: map.Id,
                    CampaignId: map.CampaignId,
                    Name: map.Name,
                    ImageData: map.ImageData,
                    Grid: new(map.GridSize, map.GridIsActive)
                );

                return Ok(payload);
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(MapDto payload)
        {
            try
            {
                var map = new DbMap()
                {
                    CampaignId = payload.CampaignId,
                    Name = payload.Name,
                    ImageData = payload.ImageData,
                    GridIsActive = payload.Grid.IsActive,
                    GridSize = payload.Grid.Size,
                };

                await _dbContext.Maps.AddAsync(map);

                var update = await _dbContext.CampaignUpdates.FirstAsync(x => x.CampaignId == payload.CampaignId);
                update.MapCollectionChange = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                await _dbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), map.Id);
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(MapDto payload)
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

                return Ok(map);
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int mapId)
        {
            try
            {
                var map = await _dbContext.Maps.FirstAsync(x => x.Id == mapId);

                var update = await _dbContext.CampaignUpdates.FirstAsync(x => x.CampaignId == map.CampaignId);
                update.MapCollectionChange = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                _dbContext.Maps.Remove(map);

                await _dbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }
        }
    }
}