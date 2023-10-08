﻿using DataTransfer.Sound;
using NSubstitute;
using Server.Models;
using Server.Services;
using Server.Services.BusinessLogic;
using System.Linq.Expressions;

namespace Server.Tests.Services.BuisnessLogic
{
    [TestClass]
    public class SoundTests
    {
        [TestMethod]
        public async Task GetActiveAmbientSound_ActiveCampaignElementsNotFound_ReturnsNull()
        {
            // Arrange
            var repository = Substitute.For<IRepository<DbActiveCampaignElements>>();
            var updateNotifier = Substitute.For<IUpdateNotifier>();
            var sound = new Sound(repository, updateNotifier);

            int campaignId = 1;
            repository.FirstAsync(Arg.Any<Expression<Func<DbActiveCampaignElements, bool>>>()).Returns(Task.FromResult<DbActiveCampaignElements?>(null));

            // Act
            var result = await sound.GetActiveAmbientSound(campaignId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [DataRow(1, -1)]
        [DataRow(3, 4)]
        [DataRow(int.MaxValue, int.MaxValue)]
        public async Task GetActiveAmbientSound_ActiveCampaignElementsFound_ReturnsDTO(int campaignId, int ambientId)
        {
            // Arrange
            var repository = Substitute.For<IRepository<DbActiveCampaignElements>>();
            var updateNotifier = Substitute.For<IUpdateNotifier>();
            var sound = new Sound(repository, updateNotifier);

            var activeCampaignElements = new DbActiveCampaignElements
            {
                CampaignId = campaignId,
                AmbientId = ambientId
            };
            repository.FirstAsync(Arg.Any<Expression<Func<DbActiveCampaignElements, bool>>>()).Returns(activeCampaignElements);

            // Act
            var result = await sound.GetActiveAmbientSound(campaignId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(activeCampaignElements.CampaignId, result.CampaignId);
            Assert.AreEqual(activeCampaignElements.AmbientId, result.AmbientId);
        }

        [TestMethod]
        public async Task GetActiveSoundEffect_ActiveCampaignElementsNotFound_ReturnsNull()
        {
            // Arrange
            var repository = Substitute.For<IRepository<DbActiveCampaignElements>>();
            var updateNotifier = Substitute.For<IUpdateNotifier>();
            var sound = new Sound(repository, updateNotifier);

            int campaignId = 1;
            repository.FirstAsync(Arg.Any<Expression<Func<DbActiveCampaignElements, bool>>>()).Returns(Task.FromResult<DbActiveCampaignElements?>(null));

            // Act
            var result = await sound.GetActiveSoundEffect(campaignId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [DataRow(1, -1)]
        [DataRow(3, 4)]
        [DataRow(int.MaxValue, int.MaxValue)]
        public async Task GetActiveSoundEffect_ActiveCampaignElementsFound_ReturnsDTO(int campaignId, int effectId)
        {
            // Arrange
            var repository = Substitute.For<IRepository<DbActiveCampaignElements>>();
            var updateNotifier = Substitute.For<IUpdateNotifier>();
            var sound = new Sound(repository, updateNotifier);

            var activeCampaignElements = new DbActiveCampaignElements
            {
                CampaignId = campaignId,
                EffectId = effectId
            };
            repository.FirstAsync(Arg.Any<Expression<Func<DbActiveCampaignElements, bool>>>()).Returns(activeCampaignElements);

            // Act
            var result = await sound.GetActiveSoundEffect(campaignId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(activeCampaignElements.CampaignId, result.CampaignId);
            Assert.AreEqual(activeCampaignElements.EffectId, result.EffectId);
        }

        [TestMethod]
        public async Task PlayAmbient_ActiveCampaignElementsNotFound_ReturnsNull()
        {
            // Arrange
            var repository = Substitute.For<IRepository<DbActiveCampaignElements>>();
            var updateNotifier = Substitute.For<IUpdateNotifier>();
            var sound = new Sound(repository, updateNotifier);

            int campaignId = 1;
            int ambientId = 1;
            repository.FirstAsync(Arg.Any<Expression<Func<DbActiveCampaignElements, bool>>>()).Returns(Task.FromResult<DbActiveCampaignElements?>(null));

            var dataTransferObject = new ActiveAmbientSoundDto(campaignId, ambientId);

            // Act
            var result = await sound.PlayAmbient(dataTransferObject);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [DataRow(1, -1)]
        [DataRow(3, 4)]
        [DataRow(int.MaxValue, int.MaxValue)]
        public async Task PlayAmbient_ActiveCampaignElementsFound_ReturnsFalse(int campaignId, int ambientId)
        {
            // Arrange
            var repository = Substitute.For<IRepository<DbActiveCampaignElements>>();
            var updateNotifier = Substitute.For<IUpdateNotifier>();
            var sound = new Sound(repository, updateNotifier);

            var activeCampaignElements = new DbActiveCampaignElements
            {
                CampaignId = campaignId,
                AmbientId = ambientId
            };
            repository.FirstAsync(Arg.Any<Expression<Func<DbActiveCampaignElements, bool>>>()).Returns(activeCampaignElements);

            var dataTransferObject = new ActiveAmbientSoundDto(campaignId, ambientId);

            // Act
            var result = await sound.PlayAmbient(dataTransferObject);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [DataRow(1, -1, 1)]
        [DataRow(1, 1, -1)]
        [DataRow(3, 4, 3)]
        [DataRow(int.MaxValue, int.MaxValue - 1, int.MaxValue)]
        public async Task PlayAmbient_ActiveCampaignElementsFound_ReturnsTrue(int campaignId, int ambientId, int differentAmbientId)
        {
            // Arrange
            var repository = Substitute.For<IRepository<DbActiveCampaignElements>>();
            var updateNotifier = Substitute.For<IUpdateNotifier>();
            var sound = new Sound(repository, updateNotifier);

            var activeCampaignElements = new DbActiveCampaignElements
            {
                CampaignId = campaignId,
                AmbientId = ambientId
            };
            repository.FirstAsync(Arg.Any<Expression<Func<DbActiveCampaignElements, bool>>>()).Returns(activeCampaignElements);

            var dataTransferObject = new ActiveAmbientSoundDto(campaignId, differentAmbientId);

            // Act
            var result = await sound.PlayAmbient(dataTransferObject);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task PlayEffect_ActiveCampaignElementsNotFound_ReturnsNull()
        {
            // Arrange
            var repository = Substitute.For<IRepository<DbActiveCampaignElements>>();
            var updateNotifier = Substitute.For<IUpdateNotifier>();
            var sound = new Sound(repository, updateNotifier);

            int campaignId = 1;
            int effectId = 1;
            repository.FirstAsync(Arg.Any<Expression<Func<DbActiveCampaignElements, bool>>>()).Returns(Task.FromResult<DbActiveCampaignElements?>(null));

            var dataTransferObject = new ActiveSoundEffectDto(campaignId, effectId);

            // Act
            var result = await sound.PlayEffect(dataTransferObject);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [DataRow(1, -1)]
        [DataRow(3, 4)]
        [DataRow(int.MaxValue, int.MaxValue)]
        public async Task PlayEffect_ActiveCampaignElementsFound_ReturnsFalse(int campaignId, int effectId)
        {
            // Arrange
            var repository = Substitute.For<IRepository<DbActiveCampaignElements>>();
            var updateNotifier = Substitute.For<IUpdateNotifier>();
            var sound = new Sound(repository, updateNotifier);

            var activeCampaignElements = new DbActiveCampaignElements
            {
                CampaignId = campaignId,
                EffectId = effectId
            };
            repository.FirstAsync(Arg.Any<Expression<Func<DbActiveCampaignElements, bool>>>()).Returns(activeCampaignElements);

            var dataTransferObject = new ActiveSoundEffectDto(campaignId, effectId);

            // Act
            var result = await sound.PlayEffect(dataTransferObject);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [DataRow(1, -1, 1)]
        [DataRow(1, 1, -1)]
        [DataRow(3, 4, 3)]
        [DataRow(int.MaxValue, int.MaxValue - 1, int.MaxValue)]
        public async Task PlayEffect_ActiveCampaignElementsFound_ReturnsTrue(int campaignId, int effectId, int differentEffectId)
        {
            // Arrange
            var repository = Substitute.For<IRepository<DbActiveCampaignElements>>();
            var updateNotifier = Substitute.For<IUpdateNotifier>();
            var sound = new Sound(repository, updateNotifier);

            var activeCampaignElements = new DbActiveCampaignElements
            {
                CampaignId = campaignId,
                EffectId = effectId
            };
            repository.FirstAsync(Arg.Any<Expression<Func<DbActiveCampaignElements, bool>>>()).Returns(activeCampaignElements);

            var dataTransferObject = new ActiveSoundEffectDto(campaignId, differentEffectId);

            // Act
            var result = await sound.PlayEffect(dataTransferObject);

            // Assert
            Assert.IsTrue(result);
        }
    }
}