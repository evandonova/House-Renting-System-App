namespace HouseRentingSystem.Services.Agents
{
    public interface IAgentService
    {
        Task<string> GetAgentIdAsync(string userId);

        Task<bool> ExistsByIdAsync(string userId);

        Task<bool> AgentWithPhoneNumberExistsAsync(string phoneNumber);

        Task CreateAsync(string userId, string phoneNumber);
    }
}
