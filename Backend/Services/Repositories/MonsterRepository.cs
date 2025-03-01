using System.Net;
using Backend.Database;
using DataTransfer.Monster;
using DataTransfer.Response;

namespace Backend.Services.Repositories;

public interface IMonsterRepository
{
    Task<Response<MonsterDto>> GetAsync(int id);
    Response<IEnumerable<MonstersDto>> GetAll();
}

public class MonsterRepository(PenAndPaperDatabase dbContext) : IMonsterRepository
{
    public async Task<Response<MonsterDto>> GetAsync(int id)
    {
        var monster = await dbContext.Monsters.FindAsync(id);

        if (monster is null)
        {
            return new Response<MonsterDto>(HttpStatusCode.NotFound);
        }

        var monsterDto = new MonsterDto(
            monster.Name,
            monster.Size,
            monster.Type,
            monster.Alignment,
            monster.ArmorClass,
            monster.HitPoints,
            monster.HitDice,
            monster.Speed,
            monster.Strength,
            monster.Dexterity,
            monster.Constitution,
            monster.Intelligence,
            monster.Wisdom,
            monster.Charisma,
            monster.SavingThrowStrength,
            monster.SavingThrowDexterity,
            monster.SavingThrowConstitution,
            monster.SavingThrowIntelligence,
            monster.SavingThrowWisdom,
            monster.SavingThrowCharisma,
            monster.Acrobatics,
            monster.AnimalHandling,
            monster.Arcana,
            monster.Athletics,
            monster.Deception,
            monster.History,
            monster.Insight,
            monster.Intimidation,
            monster.Investigation,
            monster.Medicine,
            monster.Nature,
            monster.Perception,
            monster.Performance,
            monster.Persuasion,
            monster.Religion,
            monster.SlightOfHand,
            monster.Stealth,
            monster.Survival,
            monster.DamageResistances,
            monster.DamageImmunities,
            monster.ConditionImmunities,
            monster.Senses,
            monster.Languages,
            monster.ChallengeRating,
            monster.Experience,
            monster.Actions,
            monster.Image
        );

        return new Response<MonsterDto>(HttpStatusCode.OK, monsterDto);
    }

    public Response<IEnumerable<MonstersDto>> GetAll()
    {
        var monsters = dbContext.Monsters
            .Select(m => new MonstersDto(m.Id, m.Name, m.Image, m.Size, m.Type, m.Alignment, m.ChallengeRating));

        return new Response<IEnumerable<MonstersDto>>(HttpStatusCode.OK, monsters);
    }
}