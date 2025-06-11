namespace DataTransfer.Initiative;

public record CombatantDto(int TokenId, uint Initiative, string Image, string Color);

public record CombatantUpdateDto(uint Initiative);

public record AddCombatantDto(int TokenId);

public record CombatantsUpdateDto(int TokenId);

