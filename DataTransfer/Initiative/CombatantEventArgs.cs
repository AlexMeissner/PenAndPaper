namespace DataTransfer.Initiative;

public record CombatantAddedEventArgs(int TokenId, uint Initiative, string Image, string Color);
public record CombatantRemovedEventArgs(int TokenId);
public record CombatantUpdatedEventArgs(int TokenId, uint Initiative);
public record TurnChangedEventArgs(int TokenId);
