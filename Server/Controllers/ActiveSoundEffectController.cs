using DataTransfer.Sound;
using Microsoft.AspNetCore.Mvc;
using Server.Services.BusinessLogic;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActiveSoundEffectController : ControllerBase
    {
        private readonly ISound _sound;

        public ActiveSoundEffectController(ISound sound)
        {
            _sound = sound;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int campaignId)
        {
            try
            {
                var sound = await _sound.GetActiveSoundEffect(campaignId);

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
        public async Task<IActionResult> Put(ActiveSoundEffectDto payload)
        {
            var updated = await _sound.PlayEffect(payload);

            if (updated is false)
            {
                return NotFound(payload);
            }

            return Ok(payload);
        }
    }
}
