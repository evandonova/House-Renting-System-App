using HouseRentingSystem.Data;
using HouseRentingSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingSystem.Services.Agents
{
    public class AgentService : IAgentService
    {
        private readonly HouseRentingDbContext data;

        public AgentService(HouseRentingDbContext data)
            => this.data = data;

        public async Task<string> GetAgentIdAsync(string userId)
            => (await this.data.Agents
                    .FirstAsync(a => a.UserId == userId))
                    .Id.ToString();

        public async Task<bool> ExistsByIdAsync(string userId)
            => await this.data.Agents.AnyAsync(a => a.UserId == userId);

        public async Task<bool> AgentWithPhoneNumberExistsAsync(string phoneNumber)
            => await this.data.Agents.AnyAsync(a => a.PhoneNumber == phoneNumber);

        public async Task CreateAsync(string userId, string phoneNumber)
        {
            var agent = new Agent()
            {
                UserId = userId,
                PhoneNumber = phoneNumber
            };

            await this.data.Agents.AddAsync(agent);
            await this.data.SaveChangesAsync();
        }
    }
}
