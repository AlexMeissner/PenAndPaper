using System.Threading.Channels;
using Backend.Extensions;
using Backend.Services;
using Backend.Services.Repositories;
using DataTransfer.Chat;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Chat;

[ApiController]
[Route("campaigns/{campaignId:int}")]
public class ChatController(
    IIdentity identity,
    ICampaignRepository campaignRepository,
    IUserRepository userRepository,
    Channel<ChatChannelMessage> channel)
    : ControllerBase
{
    [HttpPost("chat")]
    public async Task<IActionResult> Post(int campaignId, ChatMessageDto chatMessage)
    {
        var identityClaims = await identity.FromClaimsPrincipal(User);

        if (identityClaims is null) return Unauthorized();

        var campaignExists = await campaignRepository.ExistsAsync(campaignId);

        if (campaignExists == false) return NotFound();

        if (chatMessage.ReceiverId is { } receiverId)
        {
            var receiverExists = await userRepository.ExistsAsync(receiverId);

            if (!receiverExists) return NotFound();
        }

        var image = await userRepository.GetAvatar(identityClaims.User.Id, campaignId);

        var payload = new ChatChannelMessage(campaignId, identityClaims.User.Id, identityClaims.User.Username, image, chatMessage);

        await channel.Writer.WriteAsync(payload);

        return Ok();
    }

    [HttpGet("chat-users")]
    public async Task<IActionResult> Get(int campaignId)
    {
        var identityClaims = await identity.FromClaimsPrincipal(User);

        if (identityClaims is null) return Unauthorized();

        var response = await userRepository.GetChatUsers(identityClaims, campaignId);

        return response.Match<IActionResult>(
            Ok,
            this.StatusCode);
    }
}