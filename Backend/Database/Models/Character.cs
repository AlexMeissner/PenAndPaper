using Backend.Tokens;
using System.ComponentModel.DataAnnotations;

namespace Backend.Database.Models;

public class Character
{
    public int Id { get; init; }
    [MaxLength(32)] public required string Name { get; set; }
    public required byte[] Image { get; set; }

    public int HitPoints { get; set; }
    public int CurrentHitPoints { get; set; }
    public required string HitDice { get; set; }
    public int ArmorClass { get; set; }
    public required string Speed { get; set; }
    public required string Race { get; set; }
    public required string Class { get; set; }
    public int Level { get; set; }
    public required string Traits { get; set; }
    public required string Attacks { get; set; }
    public required string Spells { get; set; }
    public required string Inventory { get; set; }

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

    public int FirstLevelSpellSlotTotal { get; set; }
    public int FirstLevelSpellSlotUsed { get; set; }
    public int SecondLevelSpellSlotTotal { get; set; }
    public int SecondLevelSpellSlotUsed { get; set; }
    public int ThirdLevelSpellSlotTotal { get; set; }
    public int ThirdLevelSpellSlotUsed { get; set; }
    public int FourthLevelSpellSlotTotal { get; set; }
    public int FourthLevelSpellSlotUsed { get; set; }
    public int FifthLevelSpellSlotTotal { get; set; }
    public int FifthLevelSpellSlotUsed { get; set; }
    public int SixthLevelSpellSlotTotal { get; set; }
    public int SixthLevelSpellSlotUsed { get; set; }
    public int SeventhLevelSpellSlotTotal { get; set; }
    public int SeventhLevelSpellSlotUsed { get; set; }
    public int EighthLevelSpellSlotTotal { get; set; }
    public int EighthLevelSpellSlotUsed { get; set; }
    public int NinthLevelSpellSlotTotal { get; set; }
    public int NinthLevelSpellSlotUsed { get; set; }

    #region Relations

    public int UserId { get; init; }
    public required User User { get; init; }

    public int CampaignId { get; init; }

    public ICollection<CharacterToken> Tokens { get; set; } = [];

    #endregion
}