using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using static HouseRentingSystem.Data.DataConstants.House;

namespace HouseRentingSystem.Data.Entities
{
    public class House
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(AddressMaxLength)]
        public string Address { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        public string ImageUrl { get; set; } = null!;

        [Column(TypeName = "decimal(12,3)")]
        public decimal PricePerMonth { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; init; } = null!;

        public Guid AgentId { get; set; }

        public Agent Agent { get; init; } = null!;

        public string? RenterId { get; set; }

        public IdentityUser? Renter { get; set; }
    }
}
