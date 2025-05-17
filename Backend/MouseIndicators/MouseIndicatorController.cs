using Backend.Services;
using Backend.Services.Repositories;
using DataTransfer.Mouse;
using DataTransfer.Types;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Threading.Channels;

namespace Backend.MouseIndicators;

[ApiController]
[Route("campaigns/{campaignId:int}/mouse-indicators")]
public class MouseIndicatorController(
    IIdentity identity,
    ICampaignRepository campaignRepository,
    Channel<MouseIndicatorChannelMessage> mouseIndicatorChannel) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(int campaignId, MouseIndicatorDto payload)
    {
        var identityClaims = await identity.FromClaimsPrincipal(User);

        if (identityClaims is null) return Unauthorized();

        var campaignExists = await campaignRepository.ExistsAsync(campaignId);

        if (campaignExists == false) return NotFound();

        var position = new Vector2D(payload.X, payload.Y);
        var color = ToRGB(identityClaims.User.Color);
        var message = new MouseIndicatorChannelMessage(campaignId, position, color);

        await mouseIndicatorChannel.Writer.WriteAsync(message);

        return Ok();
    }

    private static Vector3D ToRGB(string hexColor)
    {
        var color = ColorTranslator.FromHtml(hexColor);
        return new Vector3D(color.R / 255.0, color.G / 255.0, color.B / 255.0);
    }
}