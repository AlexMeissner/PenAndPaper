using DataTransfer.Map;
using DataTransfer.WebSocket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MapController(SQLDatabase dbContext, IUpdateNotifier updateNotifier) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(int mapId)
        {
            var map = await dbContext.Maps.FindAsync(mapId);

            if (map is null)
            {
                return NotFound(mapId);
            }

            var payload = new MapDto(
                Id: map.Id,
                CampaignId: map.CampaignId,
                Name: map.Name,
                ImageData: map.ImageData,
                Grid: new(map.GridSize, map.GridIsActive)
            );

            return Ok(payload);
        }

        [HttpPost]
        public async Task<IActionResult> Post(MapDto payload)
        {
            var map = new Map()
            {
                CampaignId = payload.CampaignId,
                Name = payload.Name,
                ImageData = payload.ImageData,
                GridIsActive = payload.Grid.IsActive,
                GridSize = payload.Grid.Size,
                Script = string.Empty,
            };

            await dbContext.AddAsync(map);
            await dbContext.SaveChangesAsync();

            await updateNotifier.Send(payload.CampaignId, UpdateEntity.MapCollection);

            return CreatedAtAction(nameof(Get), map.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Put(MapDto payload)
        {
            var map = await dbContext.Maps.FindAsync(payload.Id);

            if (map is null)
            {
                return NotFound(payload.Id);
            }

            map.Name = payload.Name;
            map.ImageData = payload.ImageData;
            map.GridIsActive = payload.Grid.IsActive;
            map.GridSize = payload.Grid.Size;

            await dbContext.SaveChangesAsync();

            await updateNotifier.Send(payload.CampaignId, UpdateEntity.MapCollection);

            return Ok(map);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int mapId)
        {
            var map = await dbContext.Maps.FindAsync(mapId);

            if (map is null)
            {
                return NotFound(mapId);
            }

            var campaign = await dbContext.Campaigns
                .Include(c => c.ActiveMap)
                .FirstAsync(c => c.ActiveMap != null && c.ActiveMap.Id == map.Id);

            if (campaign is not null)
            {
                campaign.ActiveMap = null;
            }

            dbContext.Remove(map);

            await dbContext.SaveChangesAsync();

            await updateNotifier.Send(map.CampaignId, UpdateEntity.MapCollection);

            return Ok();
        }
    }
}
