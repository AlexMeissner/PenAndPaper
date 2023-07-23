using DataTransfer;
using DataTransfer.Sound;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Database;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SoundController : ControllerBase
    {
        private readonly SQLDatabase _dbContext;

        public SoundController(SQLDatabase dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(int id)
        {
            try
            {
                if (await _dbContext.Sounds.FirstOrDefaultAsync(x => x.Id == id) is DbSound sound)
                {
                    return Ok(new SoundDto(sound.Id, Checksum.CreateHash(sound.Data)));
                }

                return NotFound(id);
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(SoundCreationDto payload)
        {
            try
            {
                var sound = new DbSound()
                {
                    Name = payload.Name,
                    Type = payload.Type,
                    Tags = string.Join(';', payload.Tags),
                    Data = payload.Data
                };

                await _dbContext.Sounds.AddAsync(sound);
                await _dbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAsync), sound.Id);
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }
        }
    }
}
