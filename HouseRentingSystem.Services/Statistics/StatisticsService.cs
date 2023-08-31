using System.Linq;
using HouseRentingSystem.Data;
using HouseRentingSystem.Services.Statistics.Models;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingSystem.Services.Statistics
{
    public class StatisticsService : IStatisticsService
    {
        private readonly HouseRentingDbContext data;

        public StatisticsService(HouseRentingDbContext data)
            => this.data = data;

        public async Task<StatisticsServiceModel> TotalAsync()
        {
            var totalHouses = await this.data.Houses.CountAsync();
            var totalRents = await this.data.Houses
                .Where(h => h.RenterId != null).CountAsync();

            return new StatisticsServiceModel
            {
                TotalHouses = totalHouses,
                TotalRents = totalRents
            };
        }
    }
}
