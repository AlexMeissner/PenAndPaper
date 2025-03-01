namespace DataTransfer.Chat;

public enum ChatMessageType
{
    DiceRoll,
    Message
}

public enum MessageDirection
{
    Sent,
    Received
}

public record ChatMessageEventArgs(
    DateTime Timestamp,
    ChatMessageType Type,
    MessageDirection Direction,
    string Sender,
    string Text,
    string? Image,
    bool IsPrivate);

public record ChatMessageDto(int? ReceiverId, string Text);