namespace HouseRentingSystem.Services.Agents
{
    public interface IAgentService
    {
        string GetAgentId(string userId);

        bool ExistsById(string userId);

        bool AgentWithPhoneNumberExists(string phoneNumber);

        void Create(string userId, string phoneNumber);
    }
}
