using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("campaigns/{campaignId:int}/users/{userId:int}/mouse-indicators")]
public class MouseIndicatorController : ControllerBase
{
    [HttpPost]
    public IActionResult Post(int campaignId, int userId)
    {
        throw new NotImplementedException();
    }
}