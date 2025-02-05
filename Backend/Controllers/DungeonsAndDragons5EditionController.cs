using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("dungeons-and-dragons-5e/monsters")]
public class DungeonsAndDragons5EditionController : ControllerBase
{
    [HttpGet("{monsterId:int}")]
    public IActionResult Get(int monsterId)
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        throw new NotImplementedException();
    }
}