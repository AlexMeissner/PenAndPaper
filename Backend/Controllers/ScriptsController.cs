using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("campaigns/{campaignId:int}")]
public class ScriptsController : ControllerBase
{
    [HttpGet("scripts")]
    public IActionResult GetAll(int campaignId)
    {
        throw new NotImplementedException();
    }

    [HttpGet("maps/{mapId:int}/script")]
    public IActionResult Get(int campaignId, int mapId)
    {
        throw new NotImplementedException();
    }

    [HttpPut("maps/{mapId:int}/script")]
    public IActionResult Update(int campaignId, int mapId)
    {
        throw new NotImplementedException();
    }
}