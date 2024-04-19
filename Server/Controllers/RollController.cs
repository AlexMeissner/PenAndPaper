using DataTransfer.Dice;
using DataTransfer.WebSocket;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;
using System.Text.Json;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RollController(SQLDatabase dbContext, IUpdateNotifier updateNotifier) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(int campaignId)
        {
            var campaign = await dbContext.Campaigns.FindAsync(campaignId);

            if (campaign is null)
            {
                return NotFound(campaignId);
            }

            var payload = JsonSerializer.Deserialize<DiceRollResultDto>(campaign.Roll)!;

            return Ok(payload);
        }

        [HttpPut]
        public async Task<IActionResult> Put(RollDiceDto payload)
        {
            var campaign = await dbContext.Campaigns.FindAsync(payload.CampaignId);

            if (campaign is null)
            {
                return NotFound();
            }

            var random = new Random();
            int max = DiceToInt(payload.Dice);
            int roll = random.Next(1, max + 1);

            // ToDo: This causes exceptions
            var player = await dbContext.Users.FindAsync(payload.PlayerId);

            if (player is null)
            {
                return BadRequest();
            }

            var successes = new List<bool>();

            for (int i = 1; i <= max; ++i)
            {
                bool success = i <= roll;
                successes.Add(success);
            }

            var successesRandomOrder = successes.OrderBy(x => random.Next()).ToList();
            var diceRollResult = new DiceRollResultDto(player.Username, successesRandomOrder);

            campaign.Roll = JsonSerializer.Serialize(diceRollResult);

            await dbContext.SaveChangesAsync();

            await updateNotifier.Send(payload.CampaignId, UpdateEntity.Dice);

            return Ok(campaign);
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
