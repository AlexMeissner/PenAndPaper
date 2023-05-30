using DataTransfer;
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
        public async Task<ActionResult<ApiResponse<SoundOverviewDto>>> GetAsync()
        {
            try
            {
                var response = new SoundOverviewDto()
                {
                    Items = await _dbContext.Sounds.Select(x => new SoundOverviewItemDto(x.Name, x.Type, x.Tags)).ToListAsync()
                };

                return ApiResponse<SoundOverviewDto>.Success(response);
            }
            catch (Exception exception)
            {
                return ApiResponse<SoundOverviewDto>.Failure(new ErrorDetails(ErrorCode.Exception, exception.Message));
            }
        }
    }
}