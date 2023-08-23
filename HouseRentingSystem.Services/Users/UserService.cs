using HouseRentingSystem.Data;

namespace HouseRentingSystem.Services.Users
{
    public class UserService : IUserService
    {
        private readonly HouseRentingDbContext data;

        public UserService(HouseRentingDbContext data)
            => this.data = data;

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
    }
}
