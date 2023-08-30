using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HouseRentingSystem.Web.Infrastructure;
using HouseRentingSystem.Web.Models.Agents;
using HouseRentingSystem.Services.Agents;
using HouseRentingSystem.Services.Users;

namespace HouseRentingSystem.Web.Controllers
{
    public class AgentsController : Controller
    {
        private readonly IAgentService agents;
        private readonly IUserService users;

        public AgentsController(IAgentService agents, IUserService users)
        {
            this.users = users;
            this.agents = agents;
        }

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

            if (this.agents.AgentWithPhoneNumberExists(model.PhoneNumber))
            {
                ModelState.AddModelError(nameof(model.PhoneNumber),
                    "Phone number already exists. Enter another one.");
            }

            if (this.users.UserHasRents(userId))
            {
                ModelState.AddModelError("Error",
                    "You should have no rents to become an agent!");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            this.agents.Create(userId, model.PhoneNumber);

            TempData["message"] = "You have successfully become an agent!";

            return RedirectToAction(nameof(HousesController.All), "Houses");
        }
    }
}
