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
                var payload = new SoundOverviewDto()
                {
                    Items = await _dbContext.Sounds.Select(x => new SoundOverviewItemDto(x.Id, x.Name, x.Type, x.Tags)).ToListAsync()
                };

                return Ok(payload);
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }
        }
    }
}