using DataTransfer.Sound;
using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SoundOverviewController(SQLDatabase dbContext) : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var sounds = dbContext.Sounds.Select(x => new { x.Id, x.Name, x.Type, x.Tags });

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