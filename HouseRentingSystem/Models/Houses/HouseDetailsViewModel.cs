using HouseRentingSystem.Models.Agents;

namespace HouseRentingSystem.Models.Houses
{
    public class HouseDetailsViewModel : HouseViewModel
    {
        public string Description { get; init; } = null!;

        public string Category { get; init; } = null!;

        public AgentViewModel Agent { get; init; } = null!;
    }
}
