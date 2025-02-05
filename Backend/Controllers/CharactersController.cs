using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("campaigns/{campaignId:int}")]
public class CharactersController : ControllerBase
{
    [HttpGet("characters")]
    public IActionResult GetAll(int campaignId)
    {
        throw new NotImplementedException();
    }

    [HttpGet("users/{userId:int}/characters")]
    public IActionResult GetAll(int campaignId, int userId)
    {
        throw new NotImplementedException();
    }

    [HttpPost("users/{userId:int}/characters")]
    public IActionResult Post(int campaignId, int userId)
    {
        throw new NotImplementedException();
    }
}