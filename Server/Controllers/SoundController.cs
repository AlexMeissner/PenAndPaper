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
        private readonly ILogger<SoundController> _logger;

        public SoundController(SQLDatabase dbContext, ILogger<SoundController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<SoundDto>>> GetAsync(int id)
        {
            _logger.LogInformation(nameof(GetAsync));

            try
            {
                if (await _dbContext.Sounds.FirstOrDefaultAsync(x => x.Id == id) is DbSound sound)
                {
                    return ApiResponse<SoundDto>.Success(new(sound.Id, Checksum.CreateHash(sound.Data)));
                }

                return ApiResponse<SoundDto>.Failure(new ErrorDetails(ErrorCode.NoContent, $"There is no sound with id {id}."));
            }
            catch (Exception exception)
            {
                return ApiResponse<SoundDto>.Failure(new ErrorDetails(ErrorCode.Exception, exception.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> PostAsync(SoundCreationDto payload)
        {
            _logger.LogInformation(nameof(PostAsync));

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
                return this.SendResponse(ApiResponse.Success);
            }
            catch (Exception exception)
            {
                return ApiResponse.Failure(new ErrorDetails(ErrorCode.Exception, exception.Message));
            }
        }
    }
}
