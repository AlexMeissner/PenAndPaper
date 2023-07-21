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
        public async Task<IActionResult> GetAsync(int campaignId)
        {
            try
            {
                var diceRoll = await _dbContext.DiceRolls.FirstAsync(x => x.CampaignId == campaignId);
                var payload = JsonSerializer.Deserialize<DiceRollResultDto>(diceRoll.Roll)!;

                return Ok(payload);
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(RollDiceDto payload)
        {
            try
            {
                var random = new Random();
                int max = DiceToInt(payload.Dice);
                int roll = random.Next(1, max + 1);

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

                return Ok(diceRoll);
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
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