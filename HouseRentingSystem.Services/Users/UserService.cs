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

        public string? UserFullName(string userId)
        {
            var user = this.data.Users.First(u => u.Id == userId);

            if (user is null || string.IsNullOrEmpty(user.FirstName)
                || string.IsNullOrEmpty(user.LastName))
            {
                return null;
            }

            return user.FirstName + " " + user.LastName;
        }

        public bool UserHasRents(string userId)
            => this.data.Houses.Any(h => h.RenterId == userId);

        public IEnumerable<UserServiceModel> All()
        {
            var allUsers = new List<UserServiceModel>();

            var agents = this.data
                .Agents
                .Include(ag => ag.User)
                .ProjectTo<UserServiceModel>(this.mapper.ConfigurationProvider)
                .ToList();

            allUsers.AddRange(agents);

            var users = this.data
                .Users
                .Where(u => !this.data.Agents.Any(ag => ag.UserId == u.Id))
                .ProjectTo<UserServiceModel>(this.mapper.ConfigurationProvider)
                .ToList();

            allUsers.AddRange(users);

            return allUsers;
        }
    }
}
