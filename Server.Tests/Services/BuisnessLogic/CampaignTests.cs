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
                new (1, "Peter Pen", "a.b@c.com"),
                new (2, "Captain Hook", "a.b@c.net")
            };
            var playersNotInCampaign = new Collection<UsersDto>()
            {
                new(3, "Jürgen von der Rippe", "j.l@c.de"),
                new (4, "Jack Sparrow", "j.s@blackpearl.pirat")
            };

            var databaseContext = Substitute.For<IDatabaseContext>();
            var campaignRepository = Substitute.For<IRepository<Campaign>>();
            var users = Substitute.For<IRepository<User>>();
            var updateNotifier = Substitute.For<IUpdateNotifier>();

            var campaign = new CampaignManager(databaseContext, campaignRepository, users, updateNotifier);

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

            var databaseContext = Substitute.For<IDatabaseContext>();
            var campaignRepository = Substitute.For<IRepository<Campaign>>();
            var users = Substitute.For<IRepository<User>>();
            var updateNotifier = Substitute.For<IUpdateNotifier>();

            users.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>>()).Returns(Task.FromResult<User?>(null));

            var campaign = new CampaignManager(databaseContext, campaignRepository, users, updateNotifier);

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
            var dbUser = new User()
            {
                Id = userId,
                Email = "a.b@c.de",
                Username = "Jürgen",
                Password = "!§$%&/()=?`´'+~*#-_.:,;<>|@^°"
            };

            var databaseContext = Substitute.For<IDatabaseContext>();
            var campaignRepository = Substitute.For<IRepository<Campaign>>();
            var users = Substitute.For<IRepository<User>>();
            var updateNotifier = Substitute.For<IUpdateNotifier>();

            users.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>>()).Returns(dbUser);

            var campaign = new CampaignManager(databaseContext, campaignRepository, users, updateNotifier);

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
        public void GetCreationDataAsync()
        {
            // Arrange
            var databaseContext = Substitute.For<IDatabaseContext>();
            var campaignRepository = Substitute.For<IRepository<Campaign>>();
            var users = Substitute.For<IRepository<User>>();
            var updateNotifier = Substitute.For<IUpdateNotifier>();

            var campaign = new CampaignManager(databaseContext, campaignRepository, users, updateNotifier);

            // Act

            // Assert
        }

        [TestMethod]
        public void GetActiveCampaignElements()
        {
            // Arrange
            var databaseContext = Substitute.For<IDatabaseContext>();
            var campaignRepository = Substitute.For<IRepository<Campaign>>();
            var users = Substitute.For<IRepository<User>>();
            var updateNotifier = Substitute.For<IUpdateNotifier>();

            var campaign = new CampaignManager(databaseContext, campaignRepository, users, updateNotifier);

            // Act

            // Assert
        }

        [TestMethod]
        public void UpdateActiveCampaignElements()
        {
            // Arrange
            var databaseContext = Substitute.For<IDatabaseContext>();
            var campaignRepository = Substitute.For<IRepository<Campaign>>();
            var users = Substitute.For<IRepository<User>>();
            var updateNotifier = Substitute.For<IUpdateNotifier>();

            var campaign = new CampaignManager(databaseContext, campaignRepository, users, updateNotifier);

            // Act

            // Assert
        }
    }
}
