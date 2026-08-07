using DataTransfer.Types;

namespace Website.Events;

public interface ICampaignEvent;

public enum ChatMessageType
{
    DiceRoll,
    Message
}

public record ChatMessageEvent(
    DateTime Timestamp,
    ChatMessageType Type,
    int SenderId,
    string Sender,
    string Text,
    string? Image,
    bool IsPrivate) : ICampaignEvent;

public record CombatantAddedEvent(int TokenId, uint Initiative, string Image, string Color, int? CharacterId, int? MonsterId) : ICampaignEvent;

public record CombatantRemovedEvent(int TokenId) : ICampaignEvent;

public record CombatantUpdatedEvent(int TokenId, uint Initiative) : ICampaignEvent;

public record DiceRolledEvent(string Name, IEnumerable<bool> Successes) : ICampaignEvent;

public record GridChangedEvent(bool IsActive, int Size) : ICampaignEvent;

public record MapChangedEvent(int MapId) : ICampaignEvent;

public record MouseMovedEvent(Vector2D Position, Vector3D Color) : ICampaignEvent;

public record SoundStartedEvent(string Identifier, bool IsLooped, bool IsFaded) : ICampaignEvent;

public record SoundStoppedEvent(string Identifier, bool IsFaded) : ICampaignEvent;

public record TokenAddedEvent(int TokenId, int UserId, byte[] Image, int X, int Y) : ICampaignEvent;

public record TokenMovedEvent(int TokenId, int X, int Y) : ICampaignEvent;

public record TurnChangedEvent(int TokenId) : ICampaignEvent;
