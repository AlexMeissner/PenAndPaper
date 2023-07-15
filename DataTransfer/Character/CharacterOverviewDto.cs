namespace DataTransfer.Character
{
    public class CharacterOverviewItem
    {
        public int PlayerId { get; set; }
        public int CharacterId { get; set; }
        public string PlayerName { get; set; }
        public string CharacterName { get; set; }
        public string Race { get; set; }
        public string Class { get; set; }
        public byte[] Image { get; set; }
        public int Level { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int PassivePerception { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }
    }

    public class CharacterOverviewDto
    {
        public ICollection<CharacterOverviewItem> Items { get; set; }
    }
}
