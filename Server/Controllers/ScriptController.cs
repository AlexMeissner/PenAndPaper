using DataTransfer.Script;
using Microsoft.AspNetCore.Mvc;
using Server.Services.BusinessLogic;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScriptController : ControllerBase
    {
        private readonly IScript _script;

        public ScriptController(IScript script)
        {
            _script = script;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int mapId)
        {
            var script = await _script.Get(mapId);

            if (script is null)
            {
                NotFound(mapId);
            }

            return Ok(script);
        }

        [HttpPut]
        public async Task<IActionResult> Put(ScriptDto payload)
        {
            var updated = await _script.Update(payload);

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
