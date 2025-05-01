using DataTransfer.Dice;

namespace Backend.Rolls;

public record RollChannelMessage(int CampaignId, int SenderId, int? ReceiverId, string Name, Dice Dice, List<bool> Result);
