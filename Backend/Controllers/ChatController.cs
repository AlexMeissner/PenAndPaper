using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("campaigns/{campaignId:int}/chat")]
public class ChatController : ControllerBase
{
    [HttpPost]
    public IActionResult Post(int campaignId, int userId)
    {
        throw new NotImplementedException();
    }

    [HttpGet("users")]
    public IActionResult Get(int campaignId)
    {
        throw new NotImplementedException();
    }
}