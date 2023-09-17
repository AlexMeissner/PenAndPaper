namespace DataTransfer.Character
{
    public record CharacterCreationDto(
        int CampaignId,
        int UserId,
        string Name,
        Class Class,
        Race Race,
        byte[] Image,
        int Strength,
        int Dexterity,
        int Constitution,
        int Intelligence,
        int Wisdom,
        int Charisma);
}
