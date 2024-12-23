using DataTransfer.Grid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Server.Hubs;
using Server.Models;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class GridController(SQLDatabase dbContext, IHubContext<CampaignUpdateHub, ICampaignUpdate> campaignUpdateHub)
    : Controller
{
    [HttpPut]
    public async Task<IActionResult> Put(GridUpdateDto payload)
    {
        var map = await dbContext.Maps.FindAsync(payload.MapId);

        if (map is null)
        {
            return NotFound(payload.MapId);
        }

        map.GridSize = payload.Size;
        map.GridIsActive = payload.IsActive;

        await dbContext.SaveChangesAsync();

        var eventArgs = new GridChangedEventArgs(map.GridIsActive, map.GridSize);
        await campaignUpdateHub.Clients.All.GridChanged(eventArgs);

        return Ok(map);
    }
}