using Backend.Extensions;
using Backend.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("dungeons-and-dragons-5e/monsters")]
public class DungeonsAndDragons5EditionController(IMonsterRepository monsterRepository) : ControllerBase
{
    [HttpGet("{monsterId:int}")]
    public async Task<IActionResult> Get(int monsterId)
    {
        var response = await monsterRepository.GetAsync(monsterId);

        return response.Match<IActionResult>(
            Ok,
            this.StatusCode);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var response = monsterRepository.GetAll();
        
        return response.Match<IActionResult>(
            Ok,
            this.StatusCode);
    }
}