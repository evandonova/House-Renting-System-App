using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using HouseRentingSystem.Data;
using HouseRentingSystem.Services.Users.Models;

namespace HouseRentingSystem.Services.Users
{
    public class UserService : IUserService
    {
        private readonly HouseRentingDbContext data;
        private readonly IMapper mapper;

        public UserService(HouseRentingDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public async Task<string?> UserFullNameAsync(string userId)
        {
            var user = await this.data.Users.FirstAsync(u => u.Id == userId);

            if (user is null || string.IsNullOrEmpty(user.FirstName)
                || string.IsNullOrEmpty(user.LastName))
            {
                return null;
            }

            return user.FirstName + " " + user.LastName;
        }

        public async Task<bool> UserHasRentsAsync(string userId)
            => await this.data.Houses.AnyAsync(h => h.RenterId == userId);

        public async Task<IEnumerable<UserServiceModel>> AllAsync()
        {
            var allUsers = new List<UserServiceModel>();

            var agents = await this.data
                .Agents
                .Include(ag => ag.User)
                .ProjectTo<UserServiceModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            allUsers.AddRange(agents);

            var users = await this.data
                .Users
                .Where(u => !this.data.Agents.Any(ag => ag.UserId == u.Id))
                .ProjectTo<UserServiceModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            allUsers.AddRange(users);

            return allUsers;
        }
    }
}
