﻿using System.Linq;
using HouseRentingSystem.Services.Agents;
using NUnit.Framework;

namespace HouseRentingSystem.Tests.UnitTests
{
    [TestFixture]
    public class AgentServiceTests : UnitTestsBase
    {
        private IAgentService agentService;

        [OneTimeSetUp]
        public void SetUp() 
            => this.agentService = new AgentService(this.data);

        [Test]
        public async Task GetAgentId_ShouldReturnCorrectUserId()
        {
            // Arrange

            // Act: invoke the service method with valid id
            var resultAgentId = await this.agentService.GetAgentIdAsync(this.Agent.UserId);

            // Assert a correct id is returned
            Assert.That(resultAgentId, Is.EqualTo(this.Agent.Id.ToString()));
        }

        [Test]
        public async Task ExistsById_ShouldReturnTrue_WithValidId()
        {
            // Arrange

            // Act: invoke the service method with valid agent id
            var result = await this.agentService.ExistsByIdAsync(this.Agent.UserId);

            // Assert the method result is true
            Assert.IsTrue(result);
        }

        [Test]
        public async Task AgentWithPhoneNumberExists_ShouldReturnTrue_WithValidData()
        {
            // Arrange

            // Act: invoke the service method with valid agent phone num
            var result = await this.agentService
                .AgentWithPhoneNumberExistsAsync(this.Agent.PhoneNumber);

            // Assert the method result is true
            Assert.IsTrue(result);
        }

        [Test]
        public async Task CreateAgent_ShouldWorkCorrectly()
        {
            // Arrange: get all agents' current count
            var agentsCountBefore = this.data.Agents.Count();

            // Act: invoke the service method with valid data
            await this.agentService.CreateAsync(this.Agent.UserId, this.Agent.PhoneNumber);

            // Assert the agents' count has increased by 1
            var agentsCountAfter = this.data.Agents.Count();
            Assert.That(agentsCountAfter, Is.EqualTo(agentsCountBefore + 1));

            // Assert a new agent was created in the db with correct data
            var newAgentId = await this.agentService.GetAgentIdAsync(this.Agent.UserId);
            var newAgentInDb = this.data.Agents.Find(Guid.Parse(newAgentId));
            Assert.IsNotNull(newAgentInDb);
            Assert.That(newAgentInDb.UserId, Is.EqualTo(this.Agent.UserId));
            Assert.That(newAgentInDb.PhoneNumber, Is.EqualTo(this.Agent.PhoneNumber));
        }
    }
}
