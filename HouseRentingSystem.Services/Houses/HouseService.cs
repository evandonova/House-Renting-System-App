using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using HouseRentingSystem.Data;
using HouseRentingSystem.Data.Entities;
using HouseRentingSystem.Services.Users;
using HouseRentingSystem.Services.Agents.Models;
using HouseRentingSystem.Services.Houses.Models;

namespace HouseRentingSystem.Services.Houses
{
    public class HouseService : IHouseService
    {
        private readonly HouseRentingDbContext data;
        private readonly IUserService users;
        private readonly IMapper mapper;

        public HouseService(HouseRentingDbContext data, IUserService users, IMapper mapper)
        {
            this.data = data;
            this.users = users;
            this.mapper = mapper;
        }

        public async Task<HouseQueryServiceModel> AllAsync(string? category = null,
            string? searchTerm = null,
            HouseSorting sorting = HouseSorting.Newest,
            int currentPage = 1,
            int housesPerPage = 1)
        {
            var housesQuery = this.data.Houses.AsQueryable();

            if (!string.IsNullOrWhiteSpace(category))
            {
                housesQuery = this.data.Houses
                    .Where(h => h.Category.Name == category);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                housesQuery = housesQuery.Where(h =>
                    h.Title.ToLower().Contains(searchTerm.ToLower()) ||
                    h.Address.ToLower().Contains(searchTerm.ToLower()) ||
                    h.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            housesQuery = sorting switch
            {
                HouseSorting.Price => housesQuery
                    .OrderBy(h => h.PricePerMonth),
                HouseSorting.NotRentedFirst => housesQuery
                    .OrderBy(h => h.RenterId != null)
                    .ThenByDescending(h => h.Id),
                _ => housesQuery.OrderByDescending(h => h.Id)
            };

            var houses = await housesQuery
                .Skip((currentPage - 1) * housesPerPage)
                .Take(housesPerPage)
                .ProjectTo<HouseServiceModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            var totalHouses = await housesQuery.CountAsync();

            return new HouseQueryServiceModel()
            {
                TotalHousesCount = totalHouses,
                Houses = houses
            };
        }

        public async Task<IEnumerable<string>> AllCategoriesNamesAsync()
          => await this.data
               .Categories
                  .Select(c => c.Name)
                  .Distinct()
                  .ToListAsync();

        public async Task<IEnumerable<HouseServiceModel>> AllHousesByAgentIdAsync(string agentId)
        {
            var houses = await this.data
                  .Houses
                  .Where(h => h.AgentId.ToString() == agentId)
                  .ProjectTo<HouseServiceModel>(this.mapper.ConfigurationProvider)
                  .ToListAsync();

            return houses;
        }

        public async Task<IEnumerable<HouseServiceModel>> AllHousesByUserIdAsync(string userId)
        {
            var houses = await this.data
                  .Houses
                  .Where(h => h.RenterId == userId)
                  .ProjectTo<HouseServiceModel>(this.mapper.ConfigurationProvider)
                  .ToListAsync();

            return houses;
        }

        public async Task<bool> ExistsAsync(int id)
           => await this.data.Houses.AnyAsync(h => h.Id == id);

        public async Task<HouseDetailsServiceModel> HouseDetailsByIdAsync(int id)
        {
            var dbHouse = await this.data
               .Houses
               .Include(h => h.Category)
               .Include(h => h.Agent.User)
               .Where(h => h.Id == id)
               .FirstAsync();

            var house = this.mapper.Map<HouseDetailsServiceModel>(dbHouse);

            var agent = this.mapper.Map<AgentServiceModel>(dbHouse.Agent);
            agent.FullName = await this.users.UserFullNameAsync(dbHouse.Agent.UserId);

            house.Agent = agent;

            return house;
        }

        public async Task<IEnumerable<HouseCategoryServiceModel>> AllCategoriesAsync()
            => await this.data
                   .Categories
                   .ProjectTo<HouseCategoryServiceModel>(this.mapper.ConfigurationProvider)
                   .ToListAsync();

        public async Task<bool> CategoryExistsAsync(int categoryId)
            => await this.data.Categories.AnyAsync(c => c.Id == categoryId);

        public async Task<int> CreateAsync(string title, string address, string description,
            string imageUrl, decimal price, int categoryId, string agentId)
        {
            var house = new House
            {
                Title = title,
                Address = address,
                Description = description,
                ImageUrl = imageUrl,
                PricePerMonth = price,
                CategoryId = categoryId,
                AgentId = Guid.Parse(agentId)
            };

            await this.data.Houses.AddAsync(house);
            await this.data.SaveChangesAsync();

            return house.Id;
        }

        public async Task<bool> HasAgentWithIdAsync(int houseId, string currentUserId)
        {
            var house = await this.data.Houses.FirstAsync(h => h.Id == houseId);
            var agent = await this.data.Agents.FirstAsync(a => a.Id == house.AgentId);

            if (agent is null)
            {
                return false;
            }

            if (agent.UserId != currentUserId)
            {
                return false;
            }

            return true;
        }

        public async Task<int> GetHouseCategoryIdAsync(int houseId)
           => (await this.data.Houses.FirstAsync(h => h.Id == houseId)).CategoryId;

        public async Task EditAsync(int houseId, string title, string address, string description,
            string imageUrl, decimal price, int categoryId)
        {
            var house = this.data.Houses.First(h => h.Id == houseId);

            house.Title = title;
            house.Address = address;
            house.Description = description;
            house.ImageUrl = imageUrl;
            house.PricePerMonth = price;
            house.CategoryId = categoryId;

            await this.data.SaveChangesAsync();
        }

        public async Task DeleteAsync(int houseId)
        {
            var house = await this.data.Houses.FirstAsync(h => h.Id == houseId);

            this.data.Remove(house);
            await this.data.SaveChangesAsync();
        }

        public async Task<bool> IsRentedAsync(int id)
          => (await this.data.Houses.FirstAsync(h => h.Id == id)).RenterId is not null;

        public async Task<bool> IsRentedByUserWithIdAsync(int houseId, string userId)
        {
            var house = await this.data.Houses.FirstAsync(h => h.Id == houseId);

            if (house is null)
            {
                return false;
            }

            if (house.RenterId != userId)
            {
                return false;
            }

            return true;
        }

        public async Task RentAsync(int houseId, string userId)
        {
            var house = await this.data.Houses.FirstAsync(h => h.Id == houseId);

            house.RenterId = userId;
            await this.data.SaveChangesAsync();
        }

        public async Task LeaveAsync(int houseId)
        {
            var house = await this.data.Houses.FirstAsync(h => h.Id == houseId);

            house.RenterId = null;
            await this.data.SaveChangesAsync();
        }

        public async Task<IEnumerable<HouseIndexServiceModel>> LastThreeHousesAsync()
            => await this.data
                .Houses
                .OrderByDescending(c => c.Id)
                .ProjectTo<HouseIndexServiceModel>(this.mapper.ConfigurationProvider)
                .Take(3)
                .ToListAsync();
    }
}
