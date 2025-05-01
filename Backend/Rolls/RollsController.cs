using Backend.Services.Repositories;
using Backend.Services;
using DataTransfer.Dice;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Channels;
using Backend.Chat;

namespace Backend.Rolls;

[ApiController]
[Route("campaigns/{campaignId:int}/rolls")]
public class RollsController(
    IIdentity identity,
    IDiceRoller diceRoller,
    ICampaignRepository campaignRepository,
    Channel<RollChannelMessage> rollChannel,
    Channel<ChatChannelMessage> chatChannel) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(int campaignId, RollDiceDto payload)
    {
        var identityClaims = await identity.FromClaimsPrincipal(User);

        if (identityClaims is null) return Unauthorized();

        var campaignExists = await campaignRepository.ExistsAsync(campaignId);

        if (campaignExists == false) return NotFound();

        var playerId = identityClaims.User.Id;

        var rollMessage = await diceRoller.Roll(campaignId, playerId, payload);

        await rollChannel.Writer.WriteAsync(rollMessage);

        var chatMessage = await diceRoller.CreateChatMessage(campaignId, playerId, rollMessage);

        await chatChannel.Writer.WriteAsync(chatMessage);

        return Ok();
    }
}