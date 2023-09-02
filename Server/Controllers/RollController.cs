using DataTransfer.Dice;
using DataTransfer.WebSocket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Database;
using Server.Services;
using System.Text.Json;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RollController : ControllerBase
    {
        private readonly SQLDatabase _dbContext;
        private readonly IUpdateNotifier _updateNotifier;

        public RollController(SQLDatabase dbContext, IUpdateNotifier updateNotifier)
        {
            _dbContext = dbContext;
            _updateNotifier = updateNotifier;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int campaignId)
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
        public async Task<IActionResult> Put(RollDiceDto payload)
        {
            try
            {
                var random = new Random();
                int max = DiceToInt(payload.Dice);
                int roll = random.Next(1, max + 1);

                var player = await _dbContext.Users.FirstAsync(x => x.Id == payload.PlayerId);

                var successes = new List<bool>();

                for (int i = 1; i <= max; ++i)
                {
                    bool success = i <= roll;
                    successes.Add(success);
                }

                var successesRandomOrder = successes.OrderBy(x => random.Next()).ToList();
                var diceRollResult = new DiceRollResultDto(player.Username, successesRandomOrder);

                var diceRoll = await _dbContext.DiceRolls.FirstAsync(x => x.CampaignId == payload.CampaignId);
                diceRoll.Roll = JsonSerializer.Serialize(diceRollResult);

                // TODO: Fill default entry on campaign creation

                await _dbContext.SaveChangesAsync();

                await _updateNotifier.Send(payload.CampaignId, UpdateEntity.Dice);

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