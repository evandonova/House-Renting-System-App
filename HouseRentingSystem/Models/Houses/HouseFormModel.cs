using System.ComponentModel.DataAnnotations;
using HouseRentingSystem.Services.Houses.Models;

using static HouseRentingSystem.Data.DataConstants.House;

namespace HouseRentingSystem.Models.Houses
{
    public class HouseFormModel : IHouseModel
    {
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; init; } = null!;

        [Required]
        [StringLength(AddressMaxLength, MinimumLength = AddressMinLength)]
        public string Address { get; init; } = null!;

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; init; } = null!;

        [Required]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; init; } = null!;

        [Required]
        [Range(0.00, MaxPricePerMonth,
            ErrorMessage = "Price Per Month must be a positive number and less than {2} leva.")]
        [Display(Name = "Price Per Month")]
        public decimal PricePerMonth { get; init; }

        [Display(Name = "Category")]
        public int CategoryId { get; init; }

        public IEnumerable<HouseCategoryServiceModel> Categories { get; set; }
            = new List<HouseCategoryServiceModel>();
    }
}
