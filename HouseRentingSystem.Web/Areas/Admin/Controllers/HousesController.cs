using Microsoft.AspNetCore.Mvc;
using HouseRentingSystem.Web.Infrastructure;
using HouseRentingSystem.Services.Agents;
using HouseRentingSystem.Services.Houses;
using HouseRentingSystem.Web.Areas.Admin.Models;

namespace HouseRentingSystem.Web.Areas.Admin.Controllers
{
    public class HousesController : AdminController
    {
        private readonly IHouseService houses;
        private readonly IAgentService agents;

        public HousesController(IHouseService houses,
            IAgentService agents)
        {
            this.houses = houses;
            this.agents = agents;
        }

        public async Task<IActionResult> Mine()
        {
            var myHouses = new MyHousesViewModel();

            var adminUserId = this.User.Id()!;
            myHouses.RentedHouses = await this.houses.AllHousesByUserIdAsync(adminUserId);

            var adminAgentId = await this.agents.GetAgentIdAsync(adminUserId);
            myHouses.AddedHouses = await this.houses.AllHousesByAgentIdAsync(adminAgentId);

            return View(myHouses);
        }
    }
}
