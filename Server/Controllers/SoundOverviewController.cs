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
        private readonly ILogger<SoundOverviewController> _logger;

        public SoundOverviewController(SQLDatabase dbContext, ILogger<SoundOverviewController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<SoundOverviewDto>>> GetAsync()
        {
            _logger.LogInformation(nameof(GetAsync));

            try
            {
                var response = new SoundOverviewDto()
                {
                    Items = await _dbContext.Sounds.Select(x => new SoundOverviewItemDto(x.Id, x.Name, x.Type, x.Tags)).ToListAsync()
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