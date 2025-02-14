using System.ComponentModel.DataAnnotations;
using DataTransfer.Monster;

namespace Backend.Database.Models;

public class Monster
{
    public int Id { get; init; }
    [MaxLength(32)] public required string Name { get; init; }
    public SizeCategory Size { get; init; }
    [MaxLength(32)] public required string Type { get; init; }
    [MaxLength(32)] public required string Alignment { get; init; }
    public int ArmorClass { get; init; }
    public int HitPoints { get; init; }
    [MaxLength(32)] public required string HitDice { get; init; }
    [MaxLength(64)] public required string Speed { get; init; }
    public int Strength { get; init; }
    public int Dexterity { get; init; }
    public int Constitution { get; init; }
    public int Intelligence { get; init; }
    public int Wisdom { get; init; }
    public int Charisma { get; init; }
    public int SavingThrowStrength { get; init; }
    public int SavingThrowDexterity { get; init; }
    public int SavingThrowConstitution { get; init; }
    public int SavingThrowIntelligence { get; init; }
    public int SavingThrowWisdom { get; init; }
    public int SavingThrowCharisma { get; init; }
    public int Acrobatics { get; init; }
    public int AnimalHandling { get; init; }
    public int Arcana { get; init; }
    public int Athletics { get; init; }
    public int Deception { get; init; }
    public int History { get; init; }
    public int Insight { get; init; }
    public int Intimidation { get; init; }
    public int Investigation { get; init; }
    public int Medicine { get; init; }
    public int Nature { get; init; }
    public int Perception { get; init; }
    public int Performance { get; init; }
    public int Persuasion { get; init; }
    public int Religion { get; init; }
    public int SlightOfHand { get; init; }
    public int Stealth { get; init; }
    public int Survival { get; init; }
    [MaxLength(128)] public required string DamageResistances { get; init; }
    [MaxLength(128)] public required string DamageImmunities { get; init; }
    [MaxLength(128)] public required string ConditionImmunities { get; init; }
    [MaxLength(256)] public required string Senses { get; init; }
    [MaxLength(128)] public required string Languages { get; init; }
    public double ChallengeRating { get; init; }
    public int Experience { get; init; }
    [MaxLength(32)] public required string Actions { get; init; }
    public required byte[] Image { get; init; }
}