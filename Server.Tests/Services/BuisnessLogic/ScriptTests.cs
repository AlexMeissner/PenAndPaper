﻿using DataTransfer.Script;
using NSubstitute;
using Server.Models;
using Server.Services;
using Server.Services.BusinessLogic;
using System.Linq.Expressions;

namespace Server.Tests.Services.BuisnessLogic
{
    [TestClass]
    public class ScriptTests
    {
        [TestMethod]
        public async Task Get_MapNotFound_ReturnsNull()
        {
            // Arrange
            var databaseContext = Substitute.For<IDatabaseContext>();
            var repository = Substitute.For<IRepository<Map>>();
            var script = new ScriptManager(databaseContext, repository);

            repository.FirstOrDefaultAsync(Arg.Any<Expression<Func<Map, bool>>>()).Returns(Task.FromResult<Map?>(null));

            // Act
            const int campaignId = 1;
            var result = await script.Get(campaignId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow("Hello World")]
        [DataRow("Test\n\tTest")]
        [DataRow("Ça va bien! ¿Cómo estás? Привет, как дела? नमस्ते! こんにちは！안녕하세요~ Καλημέρα! مرحبًا! #@$%^&*()_+[]{}|;:'\",.<>?/`")]
        public async Task Get_MapFound_ReturnsDTO(string text)
        {
            // Arrange
            var databaseContext = Substitute.For<IDatabaseContext>();
            var repository = Substitute.For<IRepository<Map>>();
            var script = new ScriptManager(databaseContext, repository);

            var map = new Map()
            {
                Script = text,
                Name = string.Empty,
                ImageData = []
            };

            repository.FirstOrDefaultAsync(Arg.Any<Expression<Func<Map, bool>>>()).Returns(map);

            // Act
            const int campaignId = 1;
            var result = await script.Get(campaignId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(text, result.Text);
        }

        [TestMethod]
        public async Task Update_MapNotFound_ReturnsNull()
        {
            // Arrange
            var databaseContext = Substitute.For<IDatabaseContext>();
            var repository = Substitute.For<IRepository<Map>>();
            var script = new ScriptManager(databaseContext, repository);

            repository.FirstOrDefaultAsync(Arg.Any<Expression<Func<Map, bool>>>()).Returns(Task.FromResult<Map?>(null));

            const int mapId = 1;
            const string text = "";
            var payload = new ScriptDto(mapId, text);

            // Act
            var result = await script.Update(payload);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow("Hello World")]
        [DataRow("Test\n\tTest")]
        [DataRow("Ça va bien! ¿Cómo estás? Привет, как дела? नमस्ते! こんにちは！안녕하세요~ Καλημέρα! مرحبًا! #@$%^&*()_+[]{}|;:'\",.<>?/`")]
        public async Task Update_MapFound_TextIsSame_ReturnsFalse(string text)
        {
            // Arrange
            var databaseContext = Substitute.For<IDatabaseContext>();
            var repository = Substitute.For<IRepository<Map>>();
            var script = new ScriptManager(databaseContext, repository);

            var map = new Map()
            {
                Script = text,
                Name = string.Empty,
                ImageData = []
            };

            repository.FirstOrDefaultAsync(Arg.Any<Expression<Func<Map, bool>>>()).Returns(map);

            const int mapId = 1;
            var payload = new ScriptDto(mapId, text);

            // Act
            var result = await script.Update(payload);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [DataRow("", "Ça va bien! ¿Cómo estás? Привет, как дела? नमस्ते! こんにちは！안녕하세요~ Καλημέρα! مرحبًا! #@$%^&*()_+[]{}|;:'\",.<>?/`")]
        [DataRow("Hello World", "Test\n\tTest")]
        [DataRow("Test\n\tTest", "")]
        [DataRow("Ça va bien! ¿Cómo estás? Привет, как дела? नमस्ते! こんにちは！안녕하세요~ Καλημέρα! مرحبًا! #@$%^&*()_+[]{}|;:'\",.<>?/`", "Hello World")]
        public async Task Update_MapFound_TextIsDifferentSame_ReturnsTrue(string textBefore, string textAfter)
        {
            // Arrange
            var databaseContext = Substitute.For<IDatabaseContext>();
            var repository = Substitute.For<IRepository<Map>>();
            var script = new ScriptManager(databaseContext, repository);

            var map = new Map()
            {
                Script = textBefore,
                Name = string.Empty,
                ImageData = []
            };

            repository.FirstOrDefaultAsync(Arg.Any<Expression<Func<Map, bool>>>()).Returns(map);

            const int mapId = 1;
            var payload = new ScriptDto(mapId, textAfter);

            // Act
            var result = await script.Update(payload);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
