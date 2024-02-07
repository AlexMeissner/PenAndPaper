using DataTransfer;
using DataTransfer.Sound;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;

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
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if (await _dbContext.Sounds.FirstOrDefaultAsync(x => x.Id == id) is Sound sound)
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
        public async Task<IActionResult> Post(SoundCreationDto payload)
        {
            try
            {
                var sound = new Sound()
                {
                    Name = payload.Name,
                    Type = payload.Type,
                    Tags = string.Join(';', payload.Tags),
                    Data = payload.Data
                };

                await _dbContext.Sounds.AddAsync(sound);
                await _dbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), sound.Id);
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }
        }
    }
}
