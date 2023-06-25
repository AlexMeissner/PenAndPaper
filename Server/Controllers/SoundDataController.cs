using DataTransfer;
using DataTransfer.Sound;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Database;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SoundDataController : ControllerBase
    {
        private readonly SQLDatabase _dbContext;
        private readonly ILogger<SoundDataController> _logger;

        public SoundDataController(SQLDatabase dbContext, ILogger<SoundDataController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<SoundDataDto>>> GetAsync(int id)
        {
            _logger.LogInformation(nameof(GetAsync));

            try
            {
                if (await _dbContext.Sounds.FirstOrDefaultAsync(x => x.Id == id) is DbSound sound)
                {
                    return ApiResponse<SoundDataDto>.Success(new(sound.Data));
                }

                return ApiResponse<SoundDataDto>.Failure(new ErrorDetails(ErrorCode.NoContent, $"There is no sound with id {id}."));
            }
            catch (Exception exception)
            {
                return ApiResponse<SoundDataDto>.Failure(new ErrorDetails(ErrorCode.Exception, exception.Message));
            }
        }
    }
}
