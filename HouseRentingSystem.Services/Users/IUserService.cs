using HouseRentingSystem.Services.Users.Models;

namespace HouseRentingSystem.Services.Users
{
    public interface IUserService
    {
        string? UserFullName(string userId);

        IEnumerable<UserServiceModel> All();
    }
}
