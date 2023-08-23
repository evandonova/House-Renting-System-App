using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

using static HouseRentingSystem.Data.DataConstants.User;

namespace HouseRentingSystem.Data.Entities
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(UserFirstNameMaxLength)]
        public string FirstName { get; init; } = null!;

        [Required]
        [MaxLength(UserLastNameMaxLength)]
        public string LastName { get; init; } = null!;
    }
}
