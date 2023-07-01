namespace Server.Database
{
    public enum Class
    {
        // Always add new elements at the end
        Barbarian,
        Bard,
        Druid,
        Warlock,
        Fighter,
        Cleric,
        Sorcerer,
        Monk,
        Paladin,
        Rogue,
        Ranger,
        Wizard,
    }

    public enum Race
    {
        // Always add new elements at the end
        Halfling,
        Human,
        Dwarf,
        Dragonborn,
        Gnome,
        HalfElf,
        HalfOrc,
        Tiefling,
    }

    public class DbCharacter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Race Race { get; set; }
        public Class Class { get; set; }
        public int ExperiencePoints { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }
        public byte[] Image { get; set; }
    }
}
