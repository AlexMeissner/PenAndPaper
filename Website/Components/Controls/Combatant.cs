namespace Website.Components.Controls;

public abstract class Combatant
{
    public int TokenId { get; init; }
    public uint Initiative { get; set; }
    public required string Image { get; set; }
    public required string Color { get; set; }
}

public class CharacterCombatant : Combatant
{
    public int CharacterId { get; init; }
}

public class MonsterCombatant : Combatant
{
    public int MonsterId { get; init; }
}
