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

        public SoundDataController(SQLDatabase dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<SoundDataDto>>> GetAsync(int id)
        {
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
