using DataTransfer.Sound;
using Microsoft.AspNetCore.Mvc;
using Server.Services.BusinessLogic;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActiveSoundEffectController(ISoundManager soundManager) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(int campaignId)
        {
            try
            {
                var soundEffect = await soundManager.GetActiveSoundEffect(campaignId);

                if (soundEffect is null)
                {
                    return NotFound(campaignId);
                }

                return Ok(soundEffect);
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(ActiveSoundEffectDto payload)
        {
            var updated = await soundManager.PlayEffect(payload);

            if (updated is false)
            {
                return NotFound(payload);
            }

            return Ok(payload);
        }
    }
}
