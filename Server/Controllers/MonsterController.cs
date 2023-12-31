using Microsoft.AspNetCore.Mvc;
using Server.Services.BusinessLogic;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MonsterController(IMonster monsterRepository) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(int? monsterId)
        {
            if (monsterId is int id)
            {
                var monster = await monsterRepository.Get(id);

                if (monster is null)
                {
                    return NotFound(monsterId);
                }

                return Ok(monster);
            }

            var monsters = await monsterRepository.GetAll();

            return Ok(monsters);
        }
    }
}
