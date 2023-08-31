using HouseRentingSystem.Services.Users.Models;

namespace HouseRentingSystem.Services.Users
{
    public interface IUserService
    {
        Task<string?> UserFullNameAsync(string userId);

        Task<bool> UserHasRentsAsync(string userId);

        Task<IEnumerable<UserServiceModel>> AllAsync();
    }
}
