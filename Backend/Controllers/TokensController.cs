using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("campaigns/{campaignId:int}/tokens")]
public class TokensController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll(int campaignId)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{tokenId:int}")]
    public IActionResult GetAll(int tokenId, int campaignId)
    {
        throw new NotImplementedException();
    }

    [HttpPatch("{tokenId:int}")]
    public IActionResult Update(int tokenId, int campaignId)
    {
        throw new NotImplementedException();
    }

    [HttpPost("characters/{characterId:int}")]
    public IActionResult CreateCharacter(int characterId, int campaignId)
    {
        throw new NotImplementedException();
    }

    [HttpPost("monsters/{monsterId:int}")]
    public IActionResult CreateMonster(int monsterId, int campaignId)
    {
        throw new NotImplementedException();
    }
}