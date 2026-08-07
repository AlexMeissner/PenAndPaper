using System.Text.Json.Serialization;

namespace DataTransfer.Initiative;

[JsonDerivedType(typeof(CharacterCombatantDto), typeDiscriminator: nameof(CharacterCombatantDto))]
[JsonDerivedType(typeof(MonsterCombatantDto), typeDiscriminator: nameof(MonsterCombatantDto))]
public abstract record CombatantDto(int TokenId, uint Initiative, string Image, string Color);
public record CharacterCombatantDto(int TokenId, uint Initiative, string Image, string Color, int CharacterId) : CombatantDto(TokenId, Initiative, Image, Color);
public record MonsterCombatantDto(int TokenId, uint Initiative, string Image, string Color, int MonsterId) : CombatantDto(TokenId, Initiative, Image, Color);

public record CombatantUpdateDto(uint Initiative);

public record AddCombatantDto(int TokenId);

public record CombatantsUpdateDto(int TokenId);

