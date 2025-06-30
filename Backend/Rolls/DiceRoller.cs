using Backend.Chat;
using Backend.Services.Repositories;
using DataTransfer.Chat;
using DataTransfer.Dice;

namespace Backend.Rolls;

public interface IDiceRoller
{
    Task<ChatChannelMessage> CreateChatMessage(int campaignId, int playerId, RollChannelMessage message);
    Task<RollChannelMessage> Roll(int campaignId, int playerId, RollDiceDto payload);
}

public class DiceRoller(ICampaignRepository campaignRepository, IUserRepository userRepository) : IDiceRoller
{
    private static readonly Random _random = new();

    public async Task<ChatChannelMessage> CreateChatMessage(int campaignId, int senderId, RollChannelMessage rollMessage)
    {
        var image = await userRepository.GetAvatar(senderId, campaignId);

        var result = rollMessage.Result.Count(x => x == true);
        var text = $"Würfelergebnis ({rollMessage.Dice}): {result}";
        var chatMessage = new ChatMessageDto(rollMessage.ReceiverId, text);

        return new ChatChannelMessage(rollMessage.CampaignId, senderId, rollMessage.Name, image, chatMessage);
    }

    public async Task<RollChannelMessage> Roll(int campaignId, int senderId, RollDiceDto payload)
    {
        var max = DiceToInt(payload.Dice);
        var roll = _random.Next(1, max + 1);

        var successes = new List<bool>();

        for (int i = 1; i <= max; ++i)
        {
            var success = i <= roll;
            successes.Add(success);
        }

        var successesRandomOrder = successes.OrderBy(x => _random.Next()).ToList();

        var playerName = await userRepository.GetName(senderId, campaignId);

        var gamemasterId = await campaignRepository.GetGamemasterId(campaignId);
        int? receiverId = payload.IsPrivate ? gamemasterId : null;

        return new RollChannelMessage(campaignId, senderId, receiverId, playerName, payload.Dice, successesRandomOrder);
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
