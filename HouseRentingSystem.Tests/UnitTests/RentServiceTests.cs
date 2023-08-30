using HouseRentingSystem.Services.Rents;

namespace HouseRentingSystem.Tests.UnitTests
{
    [TestFixture]
    public class RentServiceTests : UnitTestsBase
    {
        private IRentService rentService;

        [OneTimeSetUp]
        public void SetUp() 
            => this.rentService = new RentService(this.data, this.mapper);

        [Test]
        public void All_ShouldReturnCorrectData()
        {
            // Arrange

            // Act: invoke the service method
            var result = this.rentService.All();

            // Assert the result is not null
            Assert.IsNotNull(result);

            // Assert the returned rents' count is correct
            var rentedHousesInDb = this.data.Houses
                .Where(h => h.RenterId != null);
            Assert.That(result.ToList().Count(), Is.EqualTo(rentedHousesInDb.Count()));

            // Assert a returned rent's data is correct
            var resultHouse = result.ToList()
                .Find(h => h.HouseTitle == this.RentedHouse.Title);
            Assert.IsNotNull(resultHouse);
            Assert.That(resultHouse.RenterEmail, Is.EqualTo(this.Renter.Email));
            Assert.That(resultHouse.RenterFullName, 
                Is.EqualTo(this.Renter.FirstName + " " + this.Renter.LastName));
            Assert.That(resultHouse.AgentEmail, Is.EqualTo(this.Agent.User.Email));
            Assert.That(resultHouse.AgentFullName, 
                Is.EqualTo(this.Agent.User.FirstName + " " + this.Agent.User.LastName));
        }
    }
}