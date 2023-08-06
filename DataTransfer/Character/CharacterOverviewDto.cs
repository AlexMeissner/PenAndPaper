namespace DataTransfer.Character
{
    public record CharacterOverviewItem
    (
        int PlayerId,
        int CharacterId,
        string PlayerName,
        string CharacterName,
        string Race,
        string Class,
        byte[] Image,
        int Level,
        int Health,
        int MaxHealth,
        int PassivePerception,
        int Strength,
        int Dexterity,
        int Constitution,
        int Intelligence,
        int Wisdom,
        int Charisma
    );

    public record CharacterOverviewDto(ICollection<CharacterOverviewItem> Items);
}
