namespace HouseRentingSystem.Services.Agents
{
    public interface IAgentService
    {
        string GetAgentId(string userId);

        bool ExistsById(string userId);

        bool UserWithPhoneNumberExists(string phoneNumber);

        bool UserHasRents(string userId);

        void Create(string userId, string phoneNumber);
    }
}
