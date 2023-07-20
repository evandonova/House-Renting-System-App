using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

using static HouseRentingSystem.Data.DataConstants.Agent;

namespace HouseRentingSystem.Data.Entities
{
    public class Agent
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        [Required]
        [MaxLength(PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;

        public IdentityUser User { get; init; } = null!;

        public IEnumerable<House> ManagedHouses { get; set; } = new List<House>();
    }
}
