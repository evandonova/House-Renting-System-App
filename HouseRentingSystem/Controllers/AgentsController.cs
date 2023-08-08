using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HouseRentingSystem.Infrastructure;
using HouseRentingSystem.Models.Agents;
using HouseRentingSystem.Services.Agents;

namespace HouseRentingSystem.Controllers
{
    public class AgentsController : Controller
    {
        private readonly IAgentService agents;

        public AgentsController(IAgentService agents) 
            => this.agents = agents;

        [Authorize]
        public IActionResult Become()
        {
            if (this.agents.ExistsById(this.User.Id()!))
            {
                return BadRequest();
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Become(BecomeAgentFormModel model)
        {
            var userId = this.User.Id()!;

            if (this.agents.ExistsById(userId))
            {
                return BadRequest();
            }

            if (this.agents.UserWithPhoneNumberExists(model.PhoneNumber))
            {
                ModelState.AddModelError(nameof(model.PhoneNumber),
                    "Phone number already exists. Enter another one.");
            }

            if (this.agents.UserHasRents(userId))
            {
                ModelState.AddModelError("Error",
                    "You should have no rents to become an agent!");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            this.agents.Create(userId, model.PhoneNumber);

            return RedirectToAction(nameof(HousesController.All), "Houses");
        }
    }
}
