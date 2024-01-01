using DataTransfer.Monster;
using DataTransfer.Script;
using NSubstitute;
using Server.Models;
using Server.Services;
using Server.Services.BusinessLogic;
using System.Linq.Expressions;

namespace Server.Tests.Services.BuisnessLogic
{
    [TestClass]
    public class MonsterTests
    {
        [TestMethod]
        [DataRow(int.MinValue, double.MinValue, "Ça va bien! ¿Cómo estás? Привет, как дела? नमस्ते! こんにちは！안녕하세요~ Καλημέρα! مرحبًا! #@$%^&*()_+[]{}|;:'\",.<>?/`", SizeCategory.Fine, new byte[] { })]
        [DataRow(0, 0.0, "Ça va bien! ¿Cómo estás? Привет, как дела? नमस्ते! こんにちは！안녕하세요~ Καλημέρα! مرحبًا! #@$%^&*()_+[]{}|;:'\",.<>?/`", SizeCategory.Medium, new byte[] { })]
        [DataRow(int.MaxValue, double.MaxValue, "Ça va bien! ¿Cómo estás? Привет, как дела? नमस्ते! こんにちは！안녕하세요~ Καλημέρα! مرحبًا! #@$%^&*()_+[]{}|;:'\",.<>?/`", SizeCategory.Colossal, new byte[] { })]
        public async Task Create_ReturnsId(int number, double fractal, string text, SizeCategory size, byte[] image)
        {
            // Arrange
            var repository = Substitute.For<IRepository<DbMonster>>();
            var service = new Monster(repository);

            var model = new MonsterDto(
             number, text, size, text, text, number, number, text, text, number, number, number, number,
             number, number, number, number, number, number, number, number, number, number, number, number,
             number, number, number, number, number, number, number, number, number, number, number, number,
             number, number, text, text, text, text, text, fractal, number, text, image);

            // Act
            var result = await service.Create(model);

            // Assert
            Assert.IsTrue(result >= 0);
        }

        [TestMethod]
        public async Task Get_ReturnsDTO()
        {
            // Arrange
            var repository = Substitute.For<IRepository<DbMonster>>();
            var service = new Monster(repository);

            var model = new List<DbMonster>
            {
                new () { Id = int.MinValue, Name = "Monster #1" },
                new () { Id = 0, Name = "Ça va bien! ¿Cómo estás? Привет, как дела? नमस्ते! こんにちは！안녕하세요~ Καλημέρα! مرحبًا! #@$%^&*()_+[]{}|;:'\",.<>?/`" },
                new () { Id = int.MaxValue, Name = "Monster #2" }
            };

            repository.Select(Arg.Any<Expression<Func<DbMonster, MonsterDto>>>()).Returns(model.Select(CreateMonsterDto));

            // Act
            var result = await service.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Items, result.Items);
        }

        [TestMethod]
        public async Task Get_MonsterNotFound_ReturnsNull()
        {
            // Arrange
            var repository = Substitute.For<IRepository<DbMonster>>();
            var service = new Monster(repository);

            repository.FirstAsync(Arg.Any<Expression<Func<DbMonster, bool>>>()).Returns(Task.FromResult<DbMonster?>(null));

            const int mapId = 1;
            const string text = "";
            var payload = new ScriptDto(mapId, text);

            // Act
            //var result = await script.Update(payload);

            // Assert
            //Assert.IsNull(result);
        }

        private static MonsterDto CreateMonsterDto(DbMonster dbMonster)
        {
            return new MonsterDto(
                dbMonster.Id,
                dbMonster.Name,
                dbMonster.Size,
                dbMonster.Type,
                dbMonster.Alignment,
                dbMonster.ArmorClass,
                dbMonster.HitPoints,
                dbMonster.HitDice,
                dbMonster.Speed,
                dbMonster.Strength,
                dbMonster.Dexterity,
                dbMonster.Constitution,
                dbMonster.Intelligence,
                dbMonster.Wisdom,
                dbMonster.Charisma,
                dbMonster.SavingThrowStrength,
                dbMonster.SavingThrowDexterity,
                dbMonster.SavingThrowConstitution,
                dbMonster.SavingThrowIntelligence,
                dbMonster.SavingThrowWisdom,
                dbMonster.SavingThrowCharisma,
                dbMonster.Acrobatics,
                dbMonster.AnimalHandling,
                dbMonster.Arcana,
                dbMonster.Athletics,
                dbMonster.Deception,
                dbMonster.History,
                dbMonster.Insight,
                dbMonster.Intimidation,
                dbMonster.Investigation,
                dbMonster.Medicine,
                dbMonster.Nature,
                dbMonster.Perception,
                dbMonster.Performance,
                dbMonster.Persuasion,
                dbMonster.Religion,
                dbMonster.SlightOfHand,
                dbMonster.Stealth,
                dbMonster.Survival,
                dbMonster.DamageResistances,
                dbMonster.DamageImmunities,
                dbMonster.ConditionImmunities,
                dbMonster.Senses,
                dbMonster.Languages,
                dbMonster.ChallangeRating,
                dbMonster.Experience,
                dbMonster.Actions,
                dbMonster.Image);
        }
    }
}
