namespace DataTransfer.Chat;

public enum ChatMessageType
{
    DiceRoll,
    Message
}

public record ChatMessage(
    DateTime Timestamp,
    ChatMessageType Type,
    int UserId,
    string Sender,
    string Text,
    string? Image,
    bool IsPrivate);