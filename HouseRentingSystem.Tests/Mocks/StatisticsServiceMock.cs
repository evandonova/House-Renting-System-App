using HouseRentingSystem.Services.Statistics;
using HouseRentingSystem.Services.Statistics.Models;
using Moq;

namespace HouseRentingSystem.Tests.Mocks
{
    public class StatisticsServiceMock
    {
        public static IStatisticsService Instance
        {
            get
            {
                var statisticsServiceMock = new Mock<IStatisticsService>();

                statisticsServiceMock
                    .Setup(s => s.TotalAsync())
                    .Returns(Task.FromResult(new StatisticsServiceModel()
                    {
                        TotalHouses = 10,
                        TotalRents = 6
                    }));

                return statisticsServiceMock.Object;
            }
        }
    }
}
