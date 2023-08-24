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

        public HouseQueryServiceModel All(string? category = null,
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

            var houses = housesQuery
                .Skip((currentPage - 1) * housesPerPage)
                .Take(housesPerPage)
                .ProjectTo<HouseServiceModel>(this.mapper.ConfigurationProvider)
                .ToList();

            var totalHouses = housesQuery.Count();

            return new HouseQueryServiceModel()
            {
                TotalHousesCount = totalHouses,
                Houses = houses
            };
        }

        public IEnumerable<string> AllCategoriesNames()
          => this.data
               .Categories
                  .Select(c => c.Name)
                  .Distinct()
                  .ToList();

        public IEnumerable<HouseServiceModel> AllHousesByAgentId(string agentId)
        {
            var houses = this.data
                  .Houses
                  .Where(h => h.AgentId.ToString() == agentId)
                  .ProjectTo<HouseServiceModel>(this.mapper.ConfigurationProvider)
                  .ToList();

            return houses;
        }

        public IEnumerable<HouseServiceModel> AllHousesByUserId(string userId)
        {
            var houses = this.data
                  .Houses
                  .Where(h => h.RenterId == userId)
                  .ProjectTo<HouseServiceModel>(this.mapper.ConfigurationProvider)
                  .ToList();

            return houses;
        }

        public bool Exists(int id)
           => this.data.Houses.Any(h => h.Id == id);

        public HouseDetailsServiceModel HouseDetailsById(int id)
        {
            var dbHouse = this.data
               .Houses
               .Include(h => h.Category)
               .Include(h => h.Agent.User)
               .Where(h => h.Id == id)
               .First();

            var house = this.mapper.Map<HouseDetailsServiceModel>(dbHouse);

            var agent = this.mapper.Map<AgentServiceModel>(dbHouse.Agent);
            agent.FullName = this.users.UserFullName(dbHouse.Agent.UserId);

            house.Agent = agent;

            return house;
        }

        public IEnumerable<HouseCategoryServiceModel> AllCategories()
            => this.data
                   .Categories
                   .ProjectTo<HouseCategoryServiceModel>(this.mapper.ConfigurationProvider)
                   .ToList();

        public bool CategoryExists(int categoryId)
            => this.data.Categories.Any(c => c.Id == categoryId);

        public int Create(string title, string address, string description,
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

            this.data.Houses.Add(house);
            this.data.SaveChanges();

            return house.Id;
        }

        public bool HasAgentWithId(int houseId, string currentUserId)
        {
            var house = this.data.Houses.First(h => h.Id == houseId);
            var agent = this.data.Agents.First(a => a.Id == house.AgentId);

            if (agent == null)
            {
                return false;
            }

            if (agent.UserId != currentUserId)
            {
                return false;
            }

            return true;
        }

        public int GetHouseCategoryId(int houseId)
           => this.data.Houses.First(h => h.Id == houseId).CategoryId;

        public void Edit(int houseId, string title, string address, string description,
            string imageUrl, decimal price, int categoryId)
        {
            var house = this.data.Houses.First(h => h.Id == houseId);

            house.Title = title;
            house.Address = address;
            house.Description = description;
            house.ImageUrl = imageUrl;
            house.PricePerMonth = price;
            house.CategoryId = categoryId;

            this.data.SaveChanges();
        }

        public void Delete(int houseId)
        {
            var house = this.data.Houses.First(h => h.Id == houseId);

            this.data.Remove(house);
            this.data.SaveChanges();
        }

        public bool IsRented(int id)
          => this.data.Houses.First(h => h.Id == id).RenterId is not null;

        public bool IsRentedByUserWithId(int houseId, string userId)
        {
            var house = this.data.Houses.First(h => h.Id == houseId);

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

        public void Rent(int houseId, string userId)
        {
            var house = this.data.Houses.First(h => h.Id == houseId);

            house.RenterId = userId;
            this.data.SaveChanges();
        }

        public void Leave(int houseId)
        {
            var house = this.data.Houses.First(h => h.Id == houseId);

            house.RenterId = null;
            this.data.SaveChanges();
        }

        public IEnumerable<HouseIndexServiceModel> LastThreeHouses()
            => this.data
                .Houses
                .OrderByDescending(c => c.Id)
                .ProjectTo<HouseIndexServiceModel>(this.mapper.ConfigurationProvider)
                .Take(3);
    }
}
