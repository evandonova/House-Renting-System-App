using AutoMapper;
using Microsoft.AspNetCore.Mvc; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using HouseRentingSystem.Web.Infrastructure;
using HouseRentingSystem.Web.Models.Houses;
using HouseRentingSystem.Services.Houses;
using HouseRentingSystem.Services.Agents;
using HouseRentingSystem.Services.Houses.Models;

using static HouseRentingSystem.Web.Areas.Admin.AdminConstants;

namespace HouseRentingSystem.Web.Controllers
{
    public class HousesController : Controller
    {
        private readonly IHouseService houses;
        private readonly IAgentService agents;
        private readonly IMapper mapper;
        private readonly IMemoryCache cache;

        public HousesController(IHouseService houses, IAgentService agents, 
            IMapper mapper, IMemoryCache cache)
        {
            this.houses = houses;
            this.agents = agents;
            this.mapper = mapper;
            this.cache = cache;
        }

        public async Task<IActionResult> All([FromQuery] AllHousesQueryModel query)
        {
            var queryResult = await this.houses.AllAsync(
                query.Category,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllHousesQueryModel.HousesPerPage);

            query.TotalHousesCount = queryResult.TotalHousesCount;
            query.Houses = queryResult.Houses;

            var houseCategories = await this.houses.AllCategoriesNamesAsync();
            query.Categories = houseCategories;

            return View(query);
        }

        [Authorize]
        public async Task<IActionResult> Mine()
        {
            if (this.User.IsInRole(AdminRoleName))
            {
                return RedirectToAction("Mine", "Houses", new { area = "Admin" });
            }

            IEnumerable<HouseServiceModel> myHouses;

            var userId = this.User.Id()!;

            if (await this.agents.ExistsByIdAsync(userId))
            {
                var currentAgentId = await this.agents.GetAgentIdAsync(userId);

                myHouses = await this.houses.AllHousesByAgentIdAsync(currentAgentId);
            }
            else
            {
                myHouses = await this.houses.AllHousesByUserIdAsync(userId);
            }

            return View(myHouses);
        }

        public async Task<IActionResult> Details(int id, string information)
        {
            if (!await this.houses.ExistsAsync(id))
            {
                return BadRequest();
            }

            var houseModel = await this.houses.HouseDetailsByIdAsync(id);

            if(information != houseModel.GetInformation())
            {
                return BadRequest();
            }

            return View(houseModel);
        }


        [Authorize]
        public async Task<IActionResult> Add()
        {
            if (!await this.agents.ExistsByIdAsync(this.User.Id()!))
            {
                return RedirectToAction(nameof(AgentsController.Become), "Agents");
            }

            return View(new HouseFormModel
            {
                Categories = await this.houses.AllCategoriesAsync()
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(HouseFormModel model)
        {
            if (!await this.agents.ExistsByIdAsync(this.User.Id()!))
            {
                return RedirectToAction(nameof(AgentsController.Become), "Agents");
            }

            if (!await this.houses.CategoryExistsAsync(model.CategoryId))
            {
                this.ModelState.AddModelError(nameof(model.CategoryId),
                    "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await this.houses.AllCategoriesAsync();

                return View(model);
            }

            var agentId = await this.agents.GetAgentIdAsync(this.User.Id()!);

            var newHouseId = this.houses.CreateAsync(model.Title, model.Address,
                model.Description, model.ImageUrl, model.PricePerMonth,
                model.CategoryId, agentId);

            TempData["message"] = "You have successfully added a house!";

            return RedirectToAction(nameof(Details),
                new { id = newHouseId, information = model.GetInformation() });
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            if (!await this.houses.ExistsAsync(id))
            {
                return BadRequest();
            }

            if (!await this.houses.HasAgentWithIdAsync(id, this.User.Id()!) 
                && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            var house = await this.houses.HouseDetailsByIdAsync(id);

            var houseCategoryId = await this.houses.GetHouseCategoryIdAsync(house.Id);

            var houseModel = this.mapper.Map<HouseFormModel>(house);
            houseModel.CategoryId = houseCategoryId;
            houseModel.Categories = await this.houses.AllCategoriesAsync();

            return View(houseModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(int id, HouseFormModel model)
        {
            if (!await this.houses.ExistsAsync(id))
            {
                return this.View();
            }

            if (!await this.houses.HasAgentWithIdAsync(id, this.User.Id()!) 
                && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            if (!await this.houses.CategoryExistsAsync(model.CategoryId))
            {
                this.ModelState.AddModelError(nameof(model.CategoryId),
                    "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await this.houses.AllCategoriesAsync();

                return View(model);
            }

            await this.houses.EditAsync(id, model.Title, model.Address, model.Description,
                model.ImageUrl, model.PricePerMonth, model.CategoryId);

            TempData["message"] = "You have successfully edited a house!";

            return RedirectToAction(nameof(Details), 
                new { id = id, information = model.GetInformation() });
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await this.houses.ExistsAsync(id))
            {
                return BadRequest();
            }

            if (!await this.houses.HasAgentWithIdAsync(id, this.User.Id()!) 
                && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            var house = this.houses.HouseDetailsByIdAsync(id);

            var model = this.mapper.Map<HouseDetailsViewModel>(house);

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(HouseDetailsViewModel model)
        {
            if (!await this.houses.ExistsAsync(model.Id))
            {
                return BadRequest();
            }

            if (!await this.houses.HasAgentWithIdAsync(model.Id, this.User.Id()!) 
                && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            await this.houses.DeleteAsync(model.Id);

            TempData["message"] = "You have successfully deleted a house!";

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Rent(int id)
        {
            if (!await this.houses.ExistsAsync(id))
            {
                return BadRequest();
            }

            if (await this.agents.ExistsByIdAsync(this.User.Id()!) 
                && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            if (await this.houses.IsRentedAsync(id))
            {
                return BadRequest();
            }

            await this.houses.RentAsync(id, this.User.Id()!);

            this.cache.Remove(RentsCacheKey);

            TempData["message"] = "You have successfully rented a house!";

            return RedirectToAction(nameof(Mine));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Leave(int id)
        {
            if (!await this.houses.ExistsAsync(id) ||
                !await this.houses.IsRentedAsync(id))
            {
                return BadRequest();
            }

            if (!await this.houses.IsRentedByUserWithIdAsync(id, this.User.Id()!))
            {
                return Unauthorized();
            }

            await this.houses.LeaveAsync(id);

            this.cache.Remove(RentsCacheKey);

            TempData["message"] = "You have successfully left a house!";

            return RedirectToAction(nameof(Mine));
        }
    }
}
