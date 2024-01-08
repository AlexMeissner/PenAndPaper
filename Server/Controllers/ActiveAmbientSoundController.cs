using DataTransfer.Sound;
using Microsoft.AspNetCore.Mvc;
using Server.Services.BusinessLogic;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActiveAmbientSoundController : ControllerBase
    {
        private readonly ISound _sound;

        public ActiveAmbientSoundController(ISound sound)
        {
            _sound = sound;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int campaignId)
        {
            try
            {
                var sound = await _sound.GetActiveAmbientSound(campaignId);

                if (sound is null)
                {
                    return NotFound(campaignId);
                }

                return Ok(sound);
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(ActiveAmbientSoundDto payload)
        {
            var updated = await _sound.PlayAmbient(payload);

            if (updated is false)
            {
                return NotFound(payload);
            }

            return Ok(payload);
        }
    }
}