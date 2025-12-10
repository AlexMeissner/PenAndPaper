using Backend.Database;
using Backend.Database.Models;
using DataTransfer.Character;
using DataTransfer.Response;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Backend.Services.Repositories;

public interface ICharacterRepository
{
    Task<Response<int>> CreateAsync(int campaignId, int userId, CharacterCreationDto payload);
    Task<Response<CharacterDto>> GetAsync(int characterId);
    Task<Response<IEnumerable<CharactersDto>>> GetAllAsync(int campaignId);
    Task<Response<IEnumerable<CharactersDto>>> GetAllAsync(int campaignId, int userId);
    Task<Response> PatchAsync(int characterId, CharacterPatchDto payload);
    Task<Response> UpdateAsync(int characterId, CharacterUpdateDto payload);
}

public class CharacterRepository(PenAndPaperDatabase dbContext) : ICharacterRepository
{
    public async Task<Response<int>> CreateAsync(int campaignId, int userId, CharacterCreationDto payload)
    {
        var user = await dbContext.Users.FindAsync(userId);

        if (user is null)
        {
            return new Response<int>(HttpStatusCode.NotFound);
        }

        var character = new Character()
        {
            Name = payload.Name,
            Image = payload.Image,
            HitPoints = payload.HitPoints,
            CurrentHitPoints = payload.HitPoints,
            HitDice = payload.HitDice,
            ArmorClass = payload.ArmorClass,
            Speed = payload.Speed,
            Race = payload.Race,
            Class = payload.Class,
            Level = payload.Level,
            Traits = payload.Traits,
            Attacks = payload.Attacks,
            Spells = payload.Spells,
            Inventory = payload.Inventory,
            Strength = payload.Strength,
            Dexterity = payload.Dexterity,
            Constitution = payload.Constitution,
            Intelligence = payload.Intelligence,
            Wisdom = payload.Wisdom,
            Charisma = payload.Charisma,
            SavingThrowStrength = payload.SavingThrowStrength,
            SavingThrowDexterity = payload.SavingThrowDexterity,
            SavingThrowConstitution = payload.SavingThrowConstitution,
            SavingThrowIntelligence = payload.SavingThrowIntelligence,
            SavingThrowWisdom = payload.SavingThrowWisdom,
            SavingThrowCharisma = payload.SavingThrowCharisma,
            Acrobatics = payload.Acrobatics,
            AnimalHandling = payload.AnimalHandling,
            Arcana = payload.Arcana,
            Athletics = payload.Athletics,
            Deception = payload.Deception,
            History = payload.History,
            Insight = payload.Insight,
            Intimidation = payload.Intimidation,
            Investigation = payload.Investigation,
            Medicine = payload.Medicine,
            Nature = payload.Nature,
            Perception = payload.Perception,
            Performance = payload.Performance,
            Persuasion = payload.Persuasion,
            Religion = payload.Religion,
            SlightOfHand = payload.SlightOfHand,
            Stealth = payload.Stealth,
            Survival = payload.Survival,
            FirstLevelSpellSlotTotal = payload.FirstLevelSpellSlot.Total,
            FirstLevelSpellSlotUsed = payload.FirstLevelSpellSlot.Used,
            SecondLevelSpellSlotTotal = payload.SecondLevelSpellSlot.Total,
            SecondLevelSpellSlotUsed = payload.SecondLevelSpellSlot.Used,
            ThirdLevelSpellSlotTotal = payload.ThirdLevelSpellSlot.Total,
            ThirdLevelSpellSlotUsed = payload.ThirdLevelSpellSlot.Used,
            FourthLevelSpellSlotTotal = payload.FourthLevelSpellSlot.Total,
            FourthLevelSpellSlotUsed = payload.FourthLevelSpellSlot.Used,
            FifthLevelSpellSlotTotal = payload.FifthLevelSpellSlot.Total,
            FifthLevelSpellSlotUsed = payload.FifthLevelSpellSlot.Used,
            SixthLevelSpellSlotTotal = payload.SixthLevelSpellSlot.Total,
            SixthLevelSpellSlotUsed = payload.SixthLevelSpellSlot.Used,
            SeventhLevelSpellSlotTotal = payload.SeventhLevelSpellSlot.Total,
            SeventhLevelSpellSlotUsed = payload.SeventhLevelSpellSlot.Used,
            EighthLevelSpellSlotTotal = payload.EighthLevelSpellSlot.Total,
            EighthLevelSpellSlotUsed = payload.EighthLevelSpellSlot.Used,
            NinthLevelSpellSlotTotal = payload.NinthLevelSpellSlot.Total,
            NinthLevelSpellSlotUsed = payload.NinthLevelSpellSlot.Used,
            User = user,
            CampaignId = campaignId
        };

        await dbContext.AddAsync(character);
        await dbContext.SaveChangesAsync();

        return new Response<int>(HttpStatusCode.Created, character.Id);
    }

    public async Task<Response<CharacterDto>> GetAsync(int characterId)
    {
        var character = await dbContext.Characters.FindAsync(characterId);

        if (character is null)
        {
            return new Response<CharacterDto>(HttpStatusCode.NotFound);
        }

        var payload = new CharacterDto(
            character.Name,
            character.ArmorClass,
            character.HitPoints,
            character.CurrentHitPoints,
            character.HitDice,
            character.Speed,
            character.Strength,
            character.Dexterity,
            character.Constitution,
            character.Intelligence,
            character.Wisdom,
            character.Charisma,
            character.SavingThrowStrength,
            character.SavingThrowDexterity,
            character.SavingThrowConstitution,
            character.SavingThrowIntelligence,
            character.SavingThrowWisdom,
            character.SavingThrowCharisma,
            character.Acrobatics,
            character.AnimalHandling,
            character.Arcana,
            character.Athletics,
            character.Deception,
            character.History,
            character.Insight,
            character.Intimidation,
            character.Investigation,
            character.Medicine,
            character.Nature,
            character.Perception,
            character.Performance,
            character.Persuasion,
            character.Religion,
            character.SlightOfHand,
            character.Stealth,
            character.Survival,
            character.Image);

        return new Response<CharacterDto>(HttpStatusCode.OK, payload);
    }

    public async Task<Response<IEnumerable<CharactersDto>>> GetAllAsync(int campaignId)
    {
        var campaign = await dbContext.Campaigns.FindAsync(campaignId);

        if (campaign == null)
        {
            return new Response<IEnumerable<CharactersDto>>(HttpStatusCode.NotFound);
        }

        var characters = dbContext.Characters
            .Where(c => c.CampaignId == campaignId)
            .Include(c => c.User)
            .Select(c => new CharactersDto(c.Id, c.Name, c.User.Username, c.Image));

        return new Response<IEnumerable<CharactersDto>>(HttpStatusCode.OK, characters);
    }

    public async Task<Response<IEnumerable<CharactersDto>>> GetAllAsync(int campaignId, int userId)
    {
        var campaign = await dbContext.Campaigns.FindAsync(campaignId);

        if (campaign == null)
        {
            return new Response<IEnumerable<CharactersDto>>(HttpStatusCode.NotFound);
        }

        var characters = dbContext.Characters
            .Where(c => c.CampaignId == campaignId && c.UserId == userId)
            .Include(c => c.User)
            .Select(c => new CharactersDto(c.Id, c.Name, c.User.Username, c.Image));

        return new Response<IEnumerable<CharactersDto>>(HttpStatusCode.OK, characters);
    }

    public async Task<Response> PatchAsync(int characterId, CharacterPatchDto payload)
    {
        var character = await dbContext.Characters.FindAsync(characterId);

        if (character is null)
        {
            return new Response(HttpStatusCode.NotFound);
        }

        if (payload.Health is { } health)
        {
            character.CurrentHitPoints = health.CurrentHitPoints;
        }

        if (payload.SpellSlot is { } spellSlot)
        {
            switch (spellSlot.Level)
            {
                case 1:
                    character.FirstLevelSpellSlotUsed = spellSlot.Used;
                    break;
                case 2:
                    character.SecondLevelSpellSlotUsed = spellSlot.Used;
                    break;
                case 3:
                    character.ThirdLevelSpellSlotUsed = spellSlot.Used;
                    break;
                case 4:
                    character.FourthLevelSpellSlotUsed = spellSlot.Used;
                    break;
                case 5:
                    character.FifthLevelSpellSlotUsed = spellSlot.Used;
                    break;
                case 6:
                    character.SixthLevelSpellSlotUsed = spellSlot.Used;
                    break;
                case 7:
                    character.SeventhLevelSpellSlotUsed = spellSlot.Used;
                    break;
                case 8:
                    character.EighthLevelSpellSlotUsed = spellSlot.Used;
                    break;
                case 9:
                    character.NinthLevelSpellSlotUsed = spellSlot.Used;
                    break;
            }
        }

        await dbContext.SaveChangesAsync();

        return new Response(HttpStatusCode.OK);
    }

    public async Task<Response> UpdateAsync(int characterId, CharacterUpdateDto payload)
    {
        var character = await dbContext.Characters.FindAsync(characterId);

        if (character is null)
        {
            return new Response(HttpStatusCode.NotFound);
        }

        character.Name = payload.Name;
        character.Image = payload.Image;
        character.HitPoints = payload.HitPoints;
        character.CurrentHitPoints = payload.HitPoints;
        character.HitDice = payload.HitDice;
        character.ArmorClass = payload.ArmorClass;
        character.Speed = payload.Speed;
        character.Race = payload.Race;
        character.Class = payload.Class;
        character.Level = payload.Level;
        character.Traits = payload.Traits;
        character.Attacks = payload.Attacks;
        character.Spells = payload.Spells;
        character.Inventory = payload.Inventory;
        character.Strength = payload.Strength;
        character.Dexterity = payload.Dexterity;
        character.Constitution = payload.Constitution;
        character.Intelligence = payload.Intelligence;
        character.Wisdom = payload.Wisdom;
        character.Charisma = payload.Charisma;
        character.SavingThrowStrength = payload.SavingThrowStrength;
        character.SavingThrowDexterity = payload.SavingThrowDexterity;
        character.SavingThrowConstitution = payload.SavingThrowConstitution;
        character.SavingThrowIntelligence = payload.SavingThrowIntelligence;
        character.SavingThrowWisdom = payload.SavingThrowWisdom;
        character.SavingThrowCharisma = payload.SavingThrowCharisma;
        character.Acrobatics = payload.Acrobatics;
        character.AnimalHandling = payload.AnimalHandling;
        character.Arcana = payload.Arcana;
        character.Athletics = payload.Athletics;
        character.Deception = payload.Deception;
        character.History = payload.History;
        character.Insight = payload.Insight;
        character.Intimidation = payload.Intimidation;
        character.Investigation = payload.Investigation;
        character.Medicine = payload.Medicine;
        character.Nature = payload.Nature;
        character.Perception = payload.Perception;
        character.Performance = payload.Performance;
        character.Persuasion = payload.Persuasion;
        character.Religion = payload.Religion;
        character.SlightOfHand = payload.SlightOfHand;
        character.Stealth = payload.Stealth;
        character.Survival = payload.Survival;
        character.FirstLevelSpellSlotTotal = payload.FirstLevelSpellSlot.Total;
        character.FirstLevelSpellSlotUsed = payload.FirstLevelSpellSlot.Used;
        character.SecondLevelSpellSlotTotal = payload.SecondLevelSpellSlot.Total;
        character.SecondLevelSpellSlotUsed = payload.SecondLevelSpellSlot.Used;
        character.ThirdLevelSpellSlotTotal = payload.ThirdLevelSpellSlot.Total;
        character.ThirdLevelSpellSlotUsed = payload.ThirdLevelSpellSlot.Used;
        character.FourthLevelSpellSlotTotal = payload.FourthLevelSpellSlot.Total;
        character.FourthLevelSpellSlotUsed = payload.FourthLevelSpellSlot.Used;
        character.FifthLevelSpellSlotTotal = payload.FifthLevelSpellSlot.Total;
        character.FifthLevelSpellSlotUsed = payload.FifthLevelSpellSlot.Used;
        character.SixthLevelSpellSlotTotal = payload.SixthLevelSpellSlot.Total;
        character.SixthLevelSpellSlotUsed = payload.SixthLevelSpellSlot.Used;
        character.SeventhLevelSpellSlotTotal = payload.SeventhLevelSpellSlot.Total;
        character.SeventhLevelSpellSlotUsed = payload.SeventhLevelSpellSlot.Used;
        character.EighthLevelSpellSlotTotal = payload.EighthLevelSpellSlot.Total;
        character.EighthLevelSpellSlotUsed = payload.EighthLevelSpellSlot.Used;
        character.NinthLevelSpellSlotTotal = payload.NinthLevelSpellSlot.Total;
        character.NinthLevelSpellSlotUsed = payload.NinthLevelSpellSlot.Used;

        await dbContext.SaveChangesAsync();

        return new Response(HttpStatusCode.OK);
    }
}