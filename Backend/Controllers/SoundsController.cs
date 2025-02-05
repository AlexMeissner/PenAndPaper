using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("campaigns/{campaignId:int}/sounds/{soundId:int}")]
public class SoundsController : ControllerBase
{
    [HttpDelete]
    public IActionResult Stop(int campaignId, int soundId)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public IActionResult Start(int campaignId, int soundId)
    {
        throw new NotImplementedException();
    }
}