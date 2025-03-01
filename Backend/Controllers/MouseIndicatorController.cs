using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("campaigns/{campaignId:int}/mouse-indicators")]
public class MouseIndicatorController : ControllerBase
{
    [HttpPost]
    public IActionResult Post(int campaignId)
    {
        throw new NotImplementedException();
    }
}