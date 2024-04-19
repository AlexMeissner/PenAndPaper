using DataTransfer.Settings;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SettingsController(IDatabaseContext dbContext, IRepository<Setting> settingRepository) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var setting = await settingRepository.FindAsync(id);

            if (setting is null)
            {
                return NotFound();
            }

            var payload = new SettingsDto(
                setting.DiceSuccessSoundId,
                setting.DiceFailSoundId,
                setting.DiceCritSuccessSoundId,
                setting.DiceCritFailSoundId);

            return Ok(payload);
        }

        [HttpPost]
        public async Task<IActionResult> Post(SettingsDto settings)
        {
            var setting = new Setting()
            {
                DiceSuccessSoundId = settings.DiceSuccessSoundId,
                DiceFailSoundId = settings.DiceFailSoundId,
                DiceCritSuccessSoundId = settings.DiceCritSuccessSoundId,
                DiceCritFailSoundId = settings.DiceCritFailSoundId
            };

            await dbContext.AddAsync(setting);

            return CreatedAtAction(nameof(Get), setting.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Put(SettingsDto settings)
        {
            var setting = await settingRepository.FirstOrDefaultAsync();

            if (setting is null)
            {
                return NotFound();
            }

            setting.DiceSuccessSoundId = settings.DiceSuccessSoundId;
            setting.DiceFailSoundId = settings.DiceFailSoundId;
            setting.DiceCritSuccessSoundId = settings.DiceCritSuccessSoundId;
            setting.DiceCritFailSoundId = settings.DiceCritFailSoundId;

            await dbContext.SaveChangesAsync();

            return Ok(setting);
        }
    }
}
