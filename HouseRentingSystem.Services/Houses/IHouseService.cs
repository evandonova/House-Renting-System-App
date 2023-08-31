using HouseRentingSystem.Services.Houses.Models;

namespace HouseRentingSystem.Services.Houses
{
    public interface IHouseService
    {
        Task<HouseQueryServiceModel> AllAsync(
          string? category = null,
          string? searchTerm = null,
          HouseSorting sorting = HouseSorting.Newest,
          int currentPage = 1,
          int housesPerPage = 1);

        Task<IEnumerable<string>> AllCategoriesNamesAsync();

        Task<IEnumerable<HouseServiceModel>> AllHousesByAgentIdAsync(string agentId);

        Task<IEnumerable<HouseServiceModel>> AllHousesByUserIdAsync(string userId);

        Task<bool> ExistsAsync(int id);

        Task<HouseDetailsServiceModel> HouseDetailsByIdAsync(int id);

        Task<IEnumerable<HouseCategoryServiceModel>> AllCategoriesAsync();

        Task<bool> CategoryExistsAsync(int categoryId);

        Task<int> CreateAsync(string title, string address,
            string description, string imageUrl, decimal price,
            int categoryId, string agentId);

        Task<bool> HasAgentWithIdAsync(int houseId, string currentUserId);

        Task<int> GetHouseCategoryIdAsync(int houseId);

        Task EditAsync(int houseId, string title, string address,
           string description, string imageUrl, decimal price, int categoryId);

        Task DeleteAsync(int houseId);

        Task<bool> IsRentedAsync(int id);

        Task<bool> IsRentedByUserWithIdAsync(int houseId, string userId);

        Task RentAsync(int houseId, string userId);

        Task LeaveAsync(int houseId);

        Task<IEnumerable<HouseIndexServiceModel>> LastThreeHousesAsync();
    }
}
