using Microsoft.AspNetCore.Mvc;
using Server.Services.BusinessLogic;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MonsterController(IMonsterManager monsterManager) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(int? monsterId)
        {
            if (monsterId is int id)
            {
                var monster = await monsterManager.Get(id);

                if (monster is null)
                {
                    return NotFound(monsterId);
                }

                return Ok(monster);
            }

            var monsters = await monsterManager.GetAll();

            return Ok(monsters);
        }
    }
}
