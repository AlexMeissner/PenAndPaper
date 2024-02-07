using DataTransfer.Script;
using Microsoft.AspNetCore.Mvc;
using Server.Services.BusinessLogic;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScriptController(IScriptManager scriptManager) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(int mapId)
        {
            var script = await scriptManager.Get(mapId);

            if (script is null)
            {
                NotFound(mapId);
            }

            return Ok(script);
        }

        [HttpPut]
        public async Task<IActionResult> Put(ScriptDto payload)
        {
            var updated = await scriptManager.Update(payload);

            if (updated is null)
            {
                return NotFound(payload);
            }

            if (updated is false)
            {
                return this.NotModified(payload);
            }

            return Ok(payload);
        }
    }
}
