using DataTransfer.Monster;

namespace MonsterManager.ViewModels
{
    internal class MonsterViewModel : BaseViewModel
    {
        public string Name { get; set; } = string.Empty;
        public SizeCategory Size { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Alignment { get; set; } = string.Empty;
        public int ArmorClass { get; set; }
        public int HitPoints { get; set; }
        public string HitDice { get; set; } = string.Empty;
        public string Speed { get; set; } = string.Empty;
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
        public string DamageResistances { get; set; } = string.Empty;
        public string DamageImmunities { get; set; } = string.Empty;
        public string ConditionImmunities { get; set; } = string.Empty;
        public string Senses { get; set; } = string.Empty;
        public string Languages { get; set; } = string.Empty;
        public double ChallengeRating { get; set; }
        public int Experience { get; set; }
        public string Actions { get; set; } = string.Empty;
        public byte[] Image { get; set; } = [];
    }
}
