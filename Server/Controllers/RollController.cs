using DataTransfer;
using DataTransfer.Dice;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Database;
using System.Text.Json;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RollController : ControllerBase
    {
        private readonly SQLDatabase _dbContext;

        public RollController(SQLDatabase dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<DiceRollResultDto>>> GetAsync(int campaignId)
        {
            try
            {
                var diceRoll = await _dbContext.DiceRolls.FirstAsync(x => x.CampaignId == campaignId);
                var response = JsonSerializer.Deserialize<DiceRollResultDto>(diceRoll.Roll)!;

                return this.SendResponse<DiceRollResultDto>(ApiResponse<DiceRollResultDto>.Success(response));
            }
            catch (Exception exception)
            {
                return ApiResponse<DiceRollResultDto>.Failure(new ErrorDetails(ErrorCode.Exception, exception.Message));
            }
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse>> PutAsync(RollDiceDto payload)
        {
            try
            {
                var random = new Random();
                int max = DiceToInt(payload.Dice);
                int roll = random.Next(1, max);

                var player = await _dbContext.Users.FirstAsync(x => x.Id == payload.PlayerId);

                DiceRollResultDto diceRollResult = new()
                {
                    Name = player.Username,
                    Succeeded = new()
                };

                for (int i = 1; i <= max; ++i)
                {
                    bool success = i <= roll;
                    diceRollResult.Succeeded.Add(success);
                }

                diceRollResult.Succeeded = diceRollResult.Succeeded.OrderBy(x => random.Next()).ToList();

                var diceRoll = await _dbContext.DiceRolls.FirstAsync(x => x.CampaignId == payload.CampaignId);
                diceRoll.Roll = JsonSerializer.Serialize(diceRollResult);

                var update = await _dbContext.CampaignUpdates.FirstAsync(x => x.CampaignId == payload.CampaignId);
                update.DiceRoll = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                // TODO: Fill default entry on campaign creation

                await _dbContext.SaveChangesAsync();

                return ApiResponse.Success;
            }
            catch (Exception exception)
            {
                return ApiResponse.Failure(new ErrorDetails(ErrorCode.Exception, exception.Message));
            }
        }

        private static int DiceToInt(Dice dice) => dice switch
        {
            Dice.D4 => 4,
            Dice.D6 => 6,
            Dice.D8 => 8,
            Dice.D10 => 10,
            Dice.D12 => 12,
            Dice.D20 => 20,
            _ => throw new ArgumentException("Dice not implemented"),
        };
    }
}