using DataTransfer.Sound;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Database;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SoundOverviewController : ControllerBase
    {
        private readonly SQLDatabase _dbContext;

        public SoundOverviewController(SQLDatabase dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var sounds = _dbContext.Sounds.Select(x => new { x.Id, x.Name, x.Type, x.Tags });

                var items = new List<SoundOverviewItemDto>();

                // ToDo: There has to be a way to do this with LINQ, but string.Split cannot be called inside LINQ Statement
                foreach (var sound in sounds)
                {
                    items.Add(new SoundOverviewItemDto(sound.Id, sound.Name, sound.Type, sound.Tags.Split(';')));
                }

                var payload = new SoundOverviewDto(items);

                return Ok(payload);
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }
        }
    }
}