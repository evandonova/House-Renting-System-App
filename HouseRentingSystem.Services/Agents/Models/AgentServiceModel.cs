namespace HouseRentingSystem.Services.Agents.Models
{
    public class AgentServiceModel
    {
        public string? FullName { get; set; }

        public string Email { get; init; } = null!;

        public string PhoneNumber { get; init; } = null!;
    }
}
