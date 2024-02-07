using DataTransfer.Monster;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Services.BusinessLogic
{
    public interface IMonsterManager
    {
        Task<int> Create(MonsterDto payload);
        Task<MonsterDto?> Get(int id);
        Task<MonstersDto> GetAll();
    }

    public class MonsterManager(IDatabaseContext dbContext, IRepository<Monster> monsterRepository) : IMonsterManager
    {
        public async Task<int> Create(MonsterDto payload)
        {
            var monster = new Monster()
            {
                Name = payload.Name,
                Size = payload.Size,
                Type = payload.Type,
                Alignment = payload.Alignment,
                ArmorClass = payload.ArmorClass,
                HitPoints = payload.HitPoints,
                HitDice = payload.HitDice,
                Speed = payload.Speed,
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
                DamageResistances = payload.DamageResistances,
                DamageImmunities = payload.DamageImmunities,
                ConditionImmunities = payload.ConditionImmunities,
                Senses = payload.Senses,
                Languages = payload.Languages,
                ChallangeRating = payload.ChallangeRating,
                Experience = payload.Experience,
                Actions = payload.Actions,
                Image = payload.Image
            };

            await dbContext.AddAsync(monster);

            return monster.Id;
        }

        public async Task<MonsterDto?> Get(int id)
        {
            var monster = await monsterRepository.FindAsync(id);

            if (monster is null)
            {
                return null;
            }

            return new(
                monster.Id,
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
                monster.ChallangeRating,
                monster.Experience,
                monster.Actions,
                monster.Image);
        }

        public async Task<MonstersDto> GetAll()
        {
            var monsters = await monsterRepository.Select(x => new MonsterDto(
                x.Id,
                x.Name,
                x.Size,
                x.Type,
                x.Alignment,
                x.ArmorClass,
                x.HitPoints,
                x.HitDice,
                x.Speed,
                x.Strength,
                x.Dexterity,
                x.Constitution,
                x.Intelligence,
                x.Wisdom,
                x.Charisma,
                x.SavingThrowStrength,
                x.SavingThrowDexterity,
                x.SavingThrowConstitution,
                x.SavingThrowIntelligence,
                x.SavingThrowWisdom,
                x.SavingThrowCharisma,
                x.Acrobatics,
                x.AnimalHandling,
                x.Arcana,
                x.Athletics,
                x.Deception,
                x.History,
                x.Insight,
                x.Intimidation,
                x.Investigation,
                x.Medicine,
                x.Nature,
                x.Perception,
                x.Performance,
                x.Persuasion,
                x.Religion,
                x.SlightOfHand,
                x.Stealth,
                x.Survival,
                x.DamageResistances,
                x.DamageImmunities,
                x.ConditionImmunities,
                x.Senses,
                x.Languages,
                x.ChallangeRating,
                x.Experience,
                x.Actions,
                x.Image)).ToListAsync();

            return new(monsters);
        }
    }
}
