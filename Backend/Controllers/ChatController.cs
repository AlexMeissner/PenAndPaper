using System.Threading.Channels;
using Backend.Extensions;
using Backend.Services;
using Backend.Services.Repositories;
using DataTransfer.Chat;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("campaigns/{campaignId:int}")]
public class ChatController(IIdentity identity, IUserRepository userRepository, Channel<ChatMessageEventArgs> channel)
    : ControllerBase
{
    [HttpPost("chat")]
    public async Task<IActionResult> Post(int campaignId, int userId)
    {
        var identityClaims = await identity.FromClaimsPrincipal(User);

        if (identityClaims is null) return Unauthorized();
        
        var payload = new ChatMessageEventArgs(
            DateTime.UtcNow,
            ChatMessageType.Message,
            MessageDirection.Sent,
            "Sender",
            "Message",
            null,
            false);

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