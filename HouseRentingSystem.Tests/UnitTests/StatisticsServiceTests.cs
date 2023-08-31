using HouseRentingSystem.Services.Statistics;

namespace HouseRentingSystem.Tests.UnitTests
{
    [TestFixture]
    public class StatisticsServiceTests : UnitTestsBase
    {
        private IStatisticsService statisticsService;

        [OneTimeSetUp]
        public void SetUp() 
            => this.statisticsService = new StatisticsService(this.data);

        [Test]
        public async Task Total_ShouldReturnCorrectCounts()
        {
            // Arrange

            // Act: invoke the service method
            var result = await this.statisticsService.TotalAsync();

            // Assert the returned result is not null
            Assert.IsNotNull(result);

            // Assert the returned houses' count is correct
            var housesCount = this.data.Houses.Count();
            Assert.That(result.TotalHouses, Is.EqualTo(housesCount));

            // Assert the returned rents' count is correct
            var rentsCount = this.data.Houses
                .Where(h => h.RenterId != null).Count();
            Assert.That(result.TotalRents, Is.EqualTo(rentsCount));
        }
    }
}
