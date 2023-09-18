using DataTransfer.Map;
using DataTransfer.WebSocket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Database;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MapController : ControllerBase
    {
        private readonly SQLDatabase _dbContext;
        private readonly IUpdateNotifier _updateNotifier;

        public MapController(SQLDatabase dbContext, IUpdateNotifier updateNotifier)
        {
            _dbContext = dbContext;
            _updateNotifier = updateNotifier;
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

                await _dbContext.SaveChangesAsync();

                await _updateNotifier.Send(payload.CampaignId, UpdateEntity.MapCollection);

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

                await _dbContext.SaveChangesAsync();

                await _updateNotifier.Send(payload.CampaignId, UpdateEntity.MapCollection);

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

                _dbContext.Maps.Remove(map);

                if (await _dbContext.ActiveCampaignElements.FirstOrDefaultAsync(x => x.MapId == mapId) is DbActiveCampaignElements activeElement)
                {
                    activeElement.MapId = -1;
                }

                await _dbContext.SaveChangesAsync();

                await _updateNotifier.Send(map.CampaignId, UpdateEntity.MapCollection);

                return Ok();
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }
        }
    }
}