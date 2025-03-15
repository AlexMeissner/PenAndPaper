using Backend.Extensions;
using Backend.Services;
using Backend.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("campaigns/{campaignId:int}")]
public class ChatController(IIdentity identity, IUserRepository userRepository) : ControllerBase
{
    [HttpPost("chat")]
    public IActionResult Post(int campaignId, int userId)
    {
        throw new NotImplementedException();
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