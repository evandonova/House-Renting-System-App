using System.ComponentModel.DataAnnotations;

using static HouseRentingSystem.Data.DataConstants.Agent;

namespace HouseRentingSystem.Web.Models.Agents
{
    public class BecomeAgentFormModel
    {
        [Required]
        [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
        [Display(Name = "Phone Number")]
        [Phone]
        public string PhoneNumber { get; init; } = null!;
    }
}
