using HouseRentingSystem.Services.Users.Models;

namespace HouseRentingSystem.Services.Users
{
    public interface IUserService
    {
        string? UserFullName(string userId);

        bool UserHasRents(string userId);

        IEnumerable<UserServiceModel> All();
    }
}
