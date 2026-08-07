using Microsoft.EntityFrameworkCore;
using Website.Database;
using Website.Database.Models;

namespace Website.Services.Repositories;

public class CharacterStats
{
    public required string Name { get; set; }
    public int ArmorClass { get; set; }
    public int HitPoints { get; set; }
    public int CurrentPoints { get; set; }
    public required string HitDice { get; set; }
    public required string Speed { get; set; }
    public required string Race { get; set; }
    public required string Class { get; set; }
    public int Level { get; set; }

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

    public required byte[] Image { get; set; }

    public required string Background { get; set; }
    public required string Traits { get; set; }
    public required string Attacks { get; set; }
    public required string Spells { get; set; }
    public required string Inventory { get; set; }
}

public record CharacterItem(int Id, string Name);

public interface ICharacterRepository
{
    Task<CharacterStats?> GetStatsAsync(int characterId);
    Task<int> CreateAsync(int campaignId, CharacterStats stats);
    Task<List<CharacterItem>> GetAllAsync(int campaignId);
    Task<List<CharacterItem>> GetAllForUserAsync(int campaignId);
    Task UpdateAsync(int characterId, CharacterStats stats);
    Task UpdateHealthAsync(int characterId, int hitPoints);
    Task UpdateSpellSlotAsync(int characterId, int level, int used);
}

public class CharacterRepository(IDbContextFactory<PenAndPaperDatabase> dbContextFactory, IUserClaims claims) : ICharacterRepository
{
    public async Task<int> CreateAsync(int campaignId, CharacterStats stats)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var user = await claims.GetUserAsync();

        var character = new Character()
        {
            Name = stats.Name,
            Image = stats.Image,
            HitPoints = stats.HitPoints,
            CurrentHitPoints = stats.HitPoints,
            HitDice = stats.HitDice,
            ArmorClass = stats.ArmorClass,
            Speed = stats.Speed,
            Race = stats.Race,
            Class = stats.Class,
            Level = stats.Level,
            Background = stats.Background,
            Traits = stats.Traits,
            Attacks = stats.Attacks,
            Spells = stats.Spells,
            Inventory = stats.Inventory,
            Strength = stats.Strength,
            Dexterity = stats.Dexterity,
            Constitution = stats.Constitution,
            Intelligence = stats.Intelligence,
            Wisdom = stats.Wisdom,
            Charisma = stats.Charisma,
            SavingThrowStrength = stats.SavingThrowStrength,
            SavingThrowDexterity = stats.SavingThrowDexterity,
            SavingThrowConstitution = stats.SavingThrowConstitution,
            SavingThrowIntelligence = stats.SavingThrowIntelligence,
            SavingThrowWisdom = stats.SavingThrowWisdom,
            SavingThrowCharisma = stats.SavingThrowCharisma,
            Acrobatics = stats.Acrobatics,
            AnimalHandling = stats.AnimalHandling,
            Arcana = stats.Arcana,
            Athletics = stats.Athletics,
            Deception = stats.Deception,
            History = stats.History,
            Insight = stats.Insight,
            Intimidation = stats.Intimidation,
            Investigation = stats.Investigation,
            Medicine = stats.Medicine,
            Nature = stats.Nature,
            Perception = stats.Perception,
            Performance = stats.Performance,
            Persuasion = stats.Persuasion,
            Religion = stats.Religion,
            SlightOfHand = stats.SlightOfHand,
            Stealth = stats.Stealth,
            Survival = stats.Survival,
            FirstLevelSpellSlotTotal = stats.FirstLevelSpellSlotTotal,
            FirstLevelSpellSlotUsed = stats.FirstLevelSpellSlotUsed,
            SecondLevelSpellSlotTotal = stats.SecondLevelSpellSlotTotal,
            SecondLevelSpellSlotUsed = stats.SecondLevelSpellSlotUsed,
            ThirdLevelSpellSlotTotal = stats.ThirdLevelSpellSlotTotal,
            ThirdLevelSpellSlotUsed = stats.ThirdLevelSpellSlotUsed,
            FourthLevelSpellSlotTotal = stats.FourthLevelSpellSlotTotal,
            FourthLevelSpellSlotUsed = stats.FourthLevelSpellSlotUsed,
            FifthLevelSpellSlotTotal = stats.FifthLevelSpellSlotTotal,
            FifthLevelSpellSlotUsed = stats.FifthLevelSpellSlotUsed,
            SixthLevelSpellSlotTotal = stats.SixthLevelSpellSlotTotal,
            SixthLevelSpellSlotUsed = stats.SixthLevelSpellSlotUsed,
            SeventhLevelSpellSlotTotal = stats.SeventhLevelSpellSlotTotal,
            SeventhLevelSpellSlotUsed = stats.SeventhLevelSpellSlotUsed,
            EighthLevelSpellSlotTotal = stats.EighthLevelSpellSlotTotal,
            EighthLevelSpellSlotUsed = stats.EighthLevelSpellSlotUsed,
            NinthLevelSpellSlotTotal = stats.NinthLevelSpellSlotTotal,
            NinthLevelSpellSlotUsed = stats.NinthLevelSpellSlotUsed,
            User = user,
            CampaignId = campaignId
        };

        await dbContext.AddAsync(character);
        await dbContext.SaveChangesAsync();

        return character.Id;
    }

    public async Task<List<CharacterItem>> GetAllAsync(int campaignId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        return await dbContext.Characters
            .AsNoTracking()
            .Where(c => c.CampaignId == campaignId)
            .Include(c => c.User)
            .Select(c => new CharacterItem(c.Id, c.Name))
            .ToListAsync();
    }

    public async Task<List<CharacterItem>> GetAllForUserAsync(int campaignId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        return await dbContext.Characters
            .Where(c => c.CampaignId == campaignId && c.UserId == claims.UserId)
            .Include(c => c.User)
            .Select(c => new CharacterItem(c.Id, c.Name))
            .ToListAsync();
    }

    public async Task<CharacterStats?> GetStatsAsync(int characterId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var character = await dbContext.Characters.FindAsync(characterId);

        if (character is null)
        {
            return null;
        }

        return new CharacterStats()
        {
            Name = character.Name,
            ArmorClass = character.ArmorClass,
            HitPoints = character.HitPoints,
            CurrentPoints = character.CurrentHitPoints,
            HitDice = character.HitDice,
            Speed = character.Speed,
            Race = character.Race,
            Class = character.Class,
            Level = character.Level,
            Strength = character.Strength,
            Dexterity = character.Dexterity,
            Constitution = character.Constitution,
            Intelligence = character.Intelligence,
            Wisdom = character.Wisdom,
            Charisma = character.Charisma,
            SavingThrowStrength = character.SavingThrowStrength,
            SavingThrowDexterity = character.SavingThrowDexterity,
            SavingThrowConstitution = character.SavingThrowConstitution,
            SavingThrowIntelligence = character.SavingThrowIntelligence,
            SavingThrowWisdom = character.SavingThrowWisdom,
            SavingThrowCharisma = character.SavingThrowCharisma,
            Acrobatics = character.Acrobatics,
            AnimalHandling = character.AnimalHandling,
            Arcana = character.Arcana,
            Athletics = character.Athletics,
            Deception = character.Deception,
            History = character.History,
            Insight = character.Insight,
            Intimidation = character.Intimidation,
            Investigation = character.Investigation,
            Medicine = character.Medicine,
            Nature = character.Nature,
            Perception = character.Perception,
            Performance = character.Performance,
            Persuasion = character.Persuasion,
            Religion = character.Religion,
            SlightOfHand = character.SlightOfHand,
            Stealth = character.Stealth,
            Survival = character.Survival,
            FirstLevelSpellSlotTotal = character.FirstLevelSpellSlotTotal,
            FirstLevelSpellSlotUsed = character.FirstLevelSpellSlotUsed,
            SecondLevelSpellSlotTotal = character.SecondLevelSpellSlotTotal,
            SecondLevelSpellSlotUsed = character.SecondLevelSpellSlotUsed,
            ThirdLevelSpellSlotTotal = character.ThirdLevelSpellSlotTotal,
            ThirdLevelSpellSlotUsed = character.ThirdLevelSpellSlotUsed,
            FourthLevelSpellSlotTotal = character.FourthLevelSpellSlotTotal,
            FourthLevelSpellSlotUsed = character.FourthLevelSpellSlotUsed,
            FifthLevelSpellSlotTotal = character.FifthLevelSpellSlotTotal,
            FifthLevelSpellSlotUsed = character.FifthLevelSpellSlotUsed,
            SixthLevelSpellSlotTotal = character.SixthLevelSpellSlotTotal,
            SixthLevelSpellSlotUsed = character.SixthLevelSpellSlotUsed,
            SeventhLevelSpellSlotTotal = character.SeventhLevelSpellSlotTotal,
            SeventhLevelSpellSlotUsed = character.SeventhLevelSpellSlotUsed,
            EighthLevelSpellSlotTotal = character.EighthLevelSpellSlotTotal,
            EighthLevelSpellSlotUsed = character.EighthLevelSpellSlotUsed,
            NinthLevelSpellSlotTotal = character.NinthLevelSpellSlotTotal,
            NinthLevelSpellSlotUsed = character.NinthLevelSpellSlotUsed,
            Image = character.Image,
            Background = character.Background,
            Traits = character.Traits,
            Attacks = character.Attacks,
            Spells = character.Spells,
            Inventory = character.Inventory
        };
    }

    public async Task UpdateAsync(int characterId, CharacterStats stats)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        if (await dbContext.Characters.FindAsync(characterId) is { } character)
        {
            character.Name = stats.Name;
            character.Image = stats.Image;
            character.HitPoints = stats.HitPoints;
            character.CurrentHitPoints = stats.HitPoints;
            character.HitDice = stats.HitDice;
            character.ArmorClass = stats.ArmorClass;
            character.Speed = stats.Speed;
            character.Race = stats.Race;
            character.Class = stats.Class;
            character.Level = stats.Level;
            character.Background = stats.Background;
            character.Traits = stats.Traits;
            character.Attacks = stats.Attacks;
            character.Spells = stats.Spells;
            character.Inventory = stats.Inventory;
            character.Strength = stats.Strength;
            character.Dexterity = stats.Dexterity;
            character.Constitution = stats.Constitution;
            character.Intelligence = stats.Intelligence;
            character.Wisdom = stats.Wisdom;
            character.Charisma = stats.Charisma;
            character.SavingThrowStrength = stats.SavingThrowStrength;
            character.SavingThrowDexterity = stats.SavingThrowDexterity;
            character.SavingThrowConstitution = stats.SavingThrowConstitution;
            character.SavingThrowIntelligence = stats.SavingThrowIntelligence;
            character.SavingThrowWisdom = stats.SavingThrowWisdom;
            character.SavingThrowCharisma = stats.SavingThrowCharisma;
            character.Acrobatics = stats.Acrobatics;
            character.AnimalHandling = stats.AnimalHandling;
            character.Arcana = stats.Arcana;
            character.Athletics = stats.Athletics;
            character.Deception = stats.Deception;
            character.History = stats.History;
            character.Insight = stats.Insight;
            character.Intimidation = stats.Intimidation;
            character.Investigation = stats.Investigation;
            character.Medicine = stats.Medicine;
            character.Nature = stats.Nature;
            character.Perception = stats.Perception;
            character.Performance = stats.Performance;
            character.Persuasion = stats.Persuasion;
            character.Religion = stats.Religion;
            character.SlightOfHand = stats.SlightOfHand;
            character.Stealth = stats.Stealth;
            character.Survival = stats.Survival;
            character.FirstLevelSpellSlotTotal = stats.FirstLevelSpellSlotTotal;
            character.FirstLevelSpellSlotUsed = stats.FirstLevelSpellSlotUsed;
            character.SecondLevelSpellSlotTotal = stats.SecondLevelSpellSlotTotal;
            character.SecondLevelSpellSlotUsed = stats.SecondLevelSpellSlotUsed;
            character.ThirdLevelSpellSlotTotal = stats.ThirdLevelSpellSlotTotal;
            character.ThirdLevelSpellSlotUsed = stats.ThirdLevelSpellSlotUsed;
            character.FourthLevelSpellSlotTotal = stats.FourthLevelSpellSlotTotal;
            character.FourthLevelSpellSlotUsed = stats.FourthLevelSpellSlotUsed;
            character.FifthLevelSpellSlotTotal = stats.FifthLevelSpellSlotTotal;
            character.FifthLevelSpellSlotUsed = stats.FifthLevelSpellSlotUsed;
            character.SixthLevelSpellSlotTotal = stats.SixthLevelSpellSlotTotal;
            character.SixthLevelSpellSlotUsed = stats.SixthLevelSpellSlotUsed;
            character.SeventhLevelSpellSlotTotal = stats.SeventhLevelSpellSlotTotal;
            character.SeventhLevelSpellSlotUsed = stats.SeventhLevelSpellSlotUsed;
            character.EighthLevelSpellSlotTotal = stats.EighthLevelSpellSlotTotal;
            character.EighthLevelSpellSlotUsed = stats.EighthLevelSpellSlotUsed;
            character.NinthLevelSpellSlotTotal = stats.NinthLevelSpellSlotTotal;
            character.NinthLevelSpellSlotUsed = stats.NinthLevelSpellSlotUsed;

            await dbContext.SaveChangesAsync();
        }
    }

    public async Task UpdateHealthAsync(int characterId, int hitPoints)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        if (await dbContext.Characters.FindAsync(characterId) is { } character)
        {
            character.CurrentHitPoints = hitPoints;
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task UpdateSpellSlotAsync(int characterId, int level, int used)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        if (await dbContext.Characters.FindAsync(characterId) is { } character)
        {
            switch (level)
            {
                case 1:
                    character.FirstLevelSpellSlotUsed = used;
                    break;
                case 2:
                    character.SecondLevelSpellSlotUsed = used;
                    break;
                case 3:
                    character.ThirdLevelSpellSlotUsed = used;
                    break;
                case 4:
                    character.FourthLevelSpellSlotUsed = used;
                    break;
                case 5:
                    character.FifthLevelSpellSlotUsed = used;
                    break;
                case 6:
                    character.SixthLevelSpellSlotUsed = used;
                    break;
                case 7:
                    character.SeventhLevelSpellSlotUsed = used;
                    break;
                case 8:
                    character.EighthLevelSpellSlotUsed = used;
                    break;
                case 9:
                    character.NinthLevelSpellSlotUsed = used;
                    break;
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
