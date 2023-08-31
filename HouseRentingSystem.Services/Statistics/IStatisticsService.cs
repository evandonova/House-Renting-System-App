using HouseRentingSystem.Services.Statistics.Models;

namespace HouseRentingSystem.Services.Statistics
{
    public interface IStatisticsService
    {
        Task<StatisticsServiceModel> TotalAsync();
    }
}
