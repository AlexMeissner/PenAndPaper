using DataTransfer.Mouse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Server.Hubs;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class MouseController(IHubContext<CampaignUpdateHub, ICampaignUpdate> campaignUpdateHub) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(MouseMoveEventArgs payload)
    {
        // ToDo: Only send to clients in campaign
        await campaignUpdateHub.Clients.All.MouseMoved(payload);
        return Ok();
    }
}