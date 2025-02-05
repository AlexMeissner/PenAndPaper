using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("campaigns/{campaignId:int}")]
public class MapsController : ControllerBase
{
    [HttpGet("active-map")]
    public IActionResult GetActiveMap(int campaignId)
    {
        throw new NotImplementedException();
    }
    
    [HttpPatch("active-map")]
    public IActionResult UpdateActiveMap(int campaignId)
    {
        throw new NotImplementedException();
    }

    [HttpPost("maps")]
    public IActionResult Create(int campaignId)
    {
        throw new NotImplementedException();
    }

    [HttpGet("maps")]
    public IActionResult GetAll(int campaignId)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("maps/{mapId:int}")]
    public IActionResult Delete(int campaignId, int mapId)
    {
        throw new NotImplementedException();
    }
    
    [HttpGet("maps/{mapId:int}")]
    public IActionResult Get(int campaignId, int mapId)
    {
        throw new NotImplementedException();
    }

    [HttpPatch("maps/{mapId:int}/grid")]
    public IActionResult UpdateGrid(int campaignId, int mapId)
    {
        throw new NotImplementedException();
    }

    [HttpPatch("maps/{mapId:int}/name")]
    public IActionResult UpdateName(int campaignId, int mapId)
    {
        throw new NotImplementedException();
    }
}