using DataTransfer.Dice;
using Website.Events;

namespace Website.Services.Repositories;

public interface IDiceRepository
{
    Task Roll(int campaignId, Dice dice);
}

public class DiceRepository(ICampaignEventHub campaignEventHub, IUserClaims claims) : IDiceRepository
{
    private static readonly Random _random = new();

    public async Task Roll(int campaignId, Dice dice)
    {
        var user = await claims.GetUserAsync();

        var max = DiceToInt(dice);
        var roll = _random.Next(1, max + 1);

        var successes = new List<bool>();

        for (int i = 1; i <= max; ++i)
        {
            var success = i <= roll;
            successes.Add(success);
        }

        var successesRandomOrder = successes.OrderBy(x => _random.Next()).ToList();

        var diceRolledEvent = new DiceRolledEvent(user.Username, successes);
        campaignEventHub.ForCampaign(campaignId).Publish(diceRolledEvent);
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
