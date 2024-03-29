﻿using DataTransfer.Monster;

namespace Server.Models
{
    public class Monster
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public SizeCategory Size { get; set; }
        public required string Type { get; set; }
        public required string Alignment { get; set; }
        public int ArmorClass { get; set; }
        public int HitPoints { get; set; }
        public required string HitDice { get; set; }
        public required string Speed { get; set; }
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
        public required string DamageResistances { get; set; }
        public required string DamageImmunities { get; set; }
        public required string ConditionImmunities { get; set; }
        public required string Senses { get; set; }
        public required string Languages { get; set; }
        public double ChallangeRating { get; set; }
        public int Experience { get; set; }
        public required string Actions { get; set; }
        public required byte[] Image { get; set; }

        #region Relationships
        public virtual ICollection<MonsterToken> Tokens { get; set; } = [];
        #endregion
    }
}
