using DataTransfer.Campaign;
using DataTransfer.User;
using NSubstitute;
using Server.Models;
using Server.Services;
using Server.Services.BusinessLogic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Server.Tests.Services.BuisnessLogic
{
    [TestClass]
    public class CamapignTests
    {
        [TestMethod]
        public async Task Create()
        {
            // Arrange
            int newCampaignId = -1;
            string campaignName = "!§$%&/()=?`´'+ ~*#-_.:,;<>|@^°";
            var gamemaster = new UsersDto(5, "Admin", "a.b@c.de");
            var playersInCampaign = new Collection<UsersDto>()
            {
                new UsersDto(1, "Peter Pen", "a.b@c.com"),
                new UsersDto(2, "Captain Hook", "a.b@c.net")
            };
            var playersNotInCampaign = new Collection<UsersDto>()
            {
                new UsersDto(3, "Jürgen von der Rippe", "j.l@c.de"),
                new UsersDto(4, "Jack Sparrow", "j.s@blackpearl.pirat")
            };

            var activeElementsRepository = Substitute.For<IRepository<DbActiveCampaignElements>>();
            var campaignRepository = Substitute.For<IRepository<DbCampaign>>();
            var users = Substitute.For<IRepository<DbUser>>();
            var campaignUsersRepository = Substitute.For<IRepository<DbUserInCampaign>>();
            var rollsRepository = Substitute.For<IRepository<DbDiceRoll>>();
            var updateNotifier = Substitute.For<IUpdateNotifier>();

            var campaign = new Campaign(activeElementsRepository, campaignRepository, users, campaignUsersRepository, rollsRepository, updateNotifier);

            var creationData = new CampaignCreationDto(newCampaignId, campaignName, gamemaster, playersNotInCampaign, playersInCampaign);

            // Act
            var campaignId = await campaign.Create(creationData);

            // Assert
            Assert.IsTrue(campaignId > 0);
        }

        [TestMethod]
        public async Task GetCreationDataAsync_NewCampaignGamemasterNotFound_ReturnsNull()
        {
            // Arrange
            int campaignId = -1;
            int userId = 2;

            var activeElementsRepository = Substitute.For<IRepository<DbActiveCampaignElements>>();
            var campaignRepository = Substitute.For<IRepository<DbCampaign>>();
            var users = Substitute.For<IRepository<DbUser>>();
            var campaignUsersRepository = Substitute.For<IRepository<DbUserInCampaign>>();
            var rollsRepository = Substitute.For<IRepository<DbDiceRoll>>();
            var updateNotifier = Substitute.For<IUpdateNotifier>();

            users.FirstAsync(Arg.Any<Expression<Func<DbUser, bool>>>()).Returns(Task.FromResult<DbUser?>(null));

            var campaign = new Campaign(activeElementsRepository, campaignRepository, users, campaignUsersRepository, rollsRepository, updateNotifier);

            // Act            
            var result = await campaign.GetCreationDataAsync(campaignId, userId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetCreationDataAsync_NewCampaignGamemasterFound_ReturnsDto()
        {
            // Arrange
            int campaignId = -1;
            int userId = 2;
            var dbUser = new DbUser()
            {
                Id = userId,
                Email = "a.b@c.de",
                Username = "Jürgen",
                Password = "!§$%&/()=?`´'+~*#-_.:,;<>|@^°"
            };

            var activeElementsRepository = Substitute.For<IRepository<DbActiveCampaignElements>>();
            var campaignRepository = Substitute.For<IRepository<DbCampaign>>();
            var users = Substitute.For<IRepository<DbUser>>();
            var campaignUsersRepository = Substitute.For<IRepository<DbUserInCampaign>>();
            var rollsRepository = Substitute.For<IRepository<DbDiceRoll>>();
            var updateNotifier = Substitute.For<IUpdateNotifier>();

            users.FirstAsync(Arg.Any<Expression<Func<DbUser, bool>>>()).Returns(dbUser);

            var campaign = new Campaign(activeElementsRepository, campaignRepository, users, campaignUsersRepository, rollsRepository, updateNotifier);

            // Act
            var result = await campaign.GetCreationDataAsync(campaignId, userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.CampaignId, campaignId);
            Assert.AreEqual(result.CampaignName, string.Empty);
            Assert.AreEqual(result.Gamemaster.Id, userId);
            Assert.AreEqual(result.Gamemaster.Id, dbUser.Id);
            Assert.AreEqual(result.Gamemaster.Username, dbUser.Username);
            Assert.AreEqual(result.Gamemaster.Email, dbUser.Email);
            Assert.AreEqual(result.UsersInCampaign.Count, 0);
            Assert.AreNotEqual(result.UsersNotInCampaign.Count, 0);
        }

        // ToDo: _users does not contain any users
        // ToDo: campaignId != -1

        [TestMethod]
        public async Task GetCreationDataAsync()
        {
            // Arrange
            var activeElementsRepository = Substitute.For<IRepository<DbActiveCampaignElements>>();
            var campaignRepository = Substitute.For<IRepository<DbCampaign>>();
            var users = Substitute.For<IRepository<DbUser>>();
            var campaignUsersRepository = Substitute.For<IRepository<DbUserInCampaign>>();
            var rollsRepository = Substitute.For<IRepository<DbDiceRoll>>();
            var updateNotifier = Substitute.For<IUpdateNotifier>();

            var campaign = new Campaign(activeElementsRepository, campaignRepository, users, campaignUsersRepository, rollsRepository, updateNotifier);

            // Act

            // Assert
        }

        [TestMethod]
        public async Task GetActiveCampaignElements()
        {
            // Arrange
            var activeElementsRepository = Substitute.For<IRepository<DbActiveCampaignElements>>();
            var campaignRepository = Substitute.For<IRepository<DbCampaign>>();
            var users = Substitute.For<IRepository<DbUser>>();
            var campaignUsersRepository = Substitute.For<IRepository<DbUserInCampaign>>();
            var rollsRepository = Substitute.For<IRepository<DbDiceRoll>>();
            var updateNotifier = Substitute.For<IUpdateNotifier>();

            var campaign = new Campaign(activeElementsRepository, campaignRepository, users, campaignUsersRepository, rollsRepository, updateNotifier);

            // Act

            // Assert
        }

        [TestMethod]
        public async Task UpdateActiveCampaignElements()
        {
            // Arrange
            var activeElementsRepository = Substitute.For<IRepository<DbActiveCampaignElements>>();
            var campaignRepository = Substitute.For<IRepository<DbCampaign>>();
            var users = Substitute.For<IRepository<DbUser>>();
            var campaignUsersRepository = Substitute.For<IRepository<DbUserInCampaign>>();
            var rollsRepository = Substitute.For<IRepository<DbDiceRoll>>();
            var updateNotifier = Substitute.For<IUpdateNotifier>();

            var campaign = new Campaign(activeElementsRepository, campaignRepository, users, campaignUsersRepository, rollsRepository, updateNotifier);

            // Act

            // Assert
        }
    }
}
