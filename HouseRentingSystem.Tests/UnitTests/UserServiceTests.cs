using HouseRentingSystem.Services.Users;

namespace HouseRentingSystem.Tests.UnitTests
{
    [TestFixture]
    public class UserServiceTests : UnitTestsBase
    {
        private IUserService userService;

        [OneTimeSetUp]
        public void SetUp() 
            => this.userService = new UserService(this.data, this.mapper);

        [Test]
        public async Task UserHasRents_ShouldReturnTrue_WithValidData()
        {
            // Arrange

            // Act: invoke the service method with valid renter id
            var result = await this.userService.UserHasRentsAsync(this.Renter.Id);

            // Assert the retunred result is true
            Assert.IsTrue(result);
        }

        [Test]
        public async Task UserFullName_ShouldReturnCorrectResult()
        {
            // Arrange

            // Act: invoke the service method with valid renter id
            var result = await this.userService.UserFullNameAsync(this.Renter.Id);

            // Assert the returned result is correct
            var renterFullName = this.Renter.FirstName + " " + 
                this.Renter.LastName;
            Assert.That(result, Is.EqualTo(renterFullName));
        }

        [Test]
        public async Task All_ShouldReturnCorrectUsersAndAgents()
        {
            // Arrange

            // Act: invoke the service method
            var result = await this.userService.AllAsync();

            // Assert the returned users' count is correct
            var usersCount = this.data.Users.Count();
            var resultUsers = result.ToList();
            Assert.That(resultUsers.Count(), Is.EqualTo(usersCount));

            // Assert the returned agents' count is correct
            var agentsCount = this.data.Agents.Count();
            var resultAgents = resultUsers.Where(us => us.PhoneNumber != "");
            Assert.That(resultAgents.Count(), Is.EqualTo(agentsCount));

            // Assert a returned agent data is correct
            var agentUser = resultAgents
                .FirstOrDefault(ag => ag.Email == this.Agent.User.Email);
            Assert.IsNotNull(agentUser);
            Assert.That(agentUser.PhoneNumber, Is.EqualTo(this.Agent.PhoneNumber));
        }
    }
}
