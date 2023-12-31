using DataTransfer.Monster;

namespace Server.Models
{
    public class DbMonster
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public SizeCategory Size { get; set; }
        public string Type { get; set; } = default!;
        public string Alignment { get; set; } = default!;
        public int ArmorClass { get; set; }
        public int HitPoints { get; set; }
        public string HitDice { get; set; } = default!;
        public string Speed { get; set; } = default!;
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }
        public int SavingThrowStrength { get; set; }
        public int SavingThrowDexterity { get; set; }
        public int SavingThrowConstitution { get; set; }
        public int SavingThrowIntelligence { get; set; }
        public int SavingThrowWisdom { get; set; }
        public int SavingThrowCharisma { get; set; }
        public int Acrobatics { get; set; }
        public int AnimalHandling { get; set; }
        public int Arcana { get; set; }
        public int Athletics { get; set; }
        public int Deception { get; set; }
        public int History { get; set; }
        public int Insight { get; set; }
        public int Intimidation { get; set; }
        public int Investigation { get; set; }
        public int Medicine { get; set; }
        public int Nature { get; set; }
        public int Perception { get; set; }
        public int Performance { get; set; }
        public int Persuasion { get; set; }
        public int Religion { get; set; }
        public int SlightOfHand { get; set; }
        public int Stealth { get; set; }
        public int Survival { get; set; }
        public string DamageResistances { get; set; } = default!;
        public string DamageImmunities { get; set; } = default!;
        public string ConditionImmunities { get; set; } = default!;
        public string Senses { get; set; } = default!;
        public string Languages { get; set; } = default!;
        public double ChallangeRating { get; set; }
        public int Experience { get; set; }
        public string Traits { get; set; } = default!;
        public string Actions { get; set; } = default!;
        public byte[] Image { get; set; } = default!;
    }
}
