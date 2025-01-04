namespace DataTransfer.Chat;

public enum ChatMessageType
{
    DiceRoll,
    Message
}

public record ChatMessageEventArgs(
    DateTime Timestamp,
    ChatMessageType Type,
    int UserId,
    string Sender,
    string Text,
    string? Image,
    bool IsPrivate);

public record ChatMessageDto(int CampaignId, int SenderId, int? ReceiverId, string Text);

public record ChatUser(int UserId, string Name);

public record ChatUsersDto(IEnumerable<ChatUser> Users);