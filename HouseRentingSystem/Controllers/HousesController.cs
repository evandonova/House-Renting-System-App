using Microsoft.AspNetCore.Mvc; 
using Microsoft.AspNetCore.Authorization;
using HouseRentingSystem.Data;
using HouseRentingSystem.Models.Houses;
using HouseRentingSystem.Data.Entities;
using HouseRentingSystem.Infrastructure;
using HouseRentingSystem.Models;

namespace HouseRentingSystem.Controllers
{
    public class HousesController : Controller
    {
        private readonly HouseRentingDbContext data;

        public HousesController(HouseRentingDbContext data)
            => this.data = data;

        public IActionResult All([FromQuery] AllHousesQueryModel query)
        {
            var housesQuery = this.data.Houses.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Category))
            {
                housesQuery = this.data.Houses
                    .Where(h => h.Category.Name == query.Category);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                housesQuery = housesQuery.Where(h =>
                    h.Title.ToLower().Contains(query.SearchTerm.ToLower()) ||
                    h.Address.ToLower().Contains(query.SearchTerm.ToLower()) ||
                    h.Description.ToLower().Contains(query.SearchTerm.ToLower()));
            }

            housesQuery = query.Sorting switch
            {
                HouseSorting.Price => housesQuery.OrderBy(h => h.PricePerMonth),
                HouseSorting.NotRentedFirst => housesQuery.OrderBy(h => h.RenterId != null)
                                                          .ThenByDescending(h => h.Id),
                _ => housesQuery.OrderByDescending(h => h.Id)
            };

            var houses = housesQuery
                .Skip((query.CurrentPage - 1) * AllHousesQueryModel.HousesPerPage)
                .Take(AllHousesQueryModel.HousesPerPage)
                .Select(h => new HouseViewModel
                {
                    Id = h.Id,
                    Title = h.Title,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    IsRented = h.RenterId != null,
                    PricePerMonth = h.PricePerMonth
                })
                .ToList();

            query.Houses = houses;

            var houseCategories = this.data
                .Categories
                .Select(c => c.Name)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            query.Categories = houseCategories;

            var totalHouses = housesQuery.Count();
            query.TotalHousesCount = totalHouses;

            return View(query);
        }

        [Authorize]
        public IActionResult Mine()
        {
            //var allHouses = new AllHousesQueryModel()
            //{
            //    Houses = this.data.Houses
            //        .Where(h => h.Agent.UserId == this.User.Id())
            //        .Select(h => new HouseDetailsViewModel()
            //        {
            //            Title = h.Title,
            //            Address = h.Address,
            //            ImageUrl = h.ImageUrl
            //        })
            //};

            return View(/*allHouses*/);
        }

        public IActionResult Details(int id)
        {
            var house = this.data.Houses.Find(id);

            if (house is null)
            {
                return BadRequest();
            }

            var houseModel = new HouseDetailsViewModel()
            {
                Title = house.Title,
                Address = house.Address,
                ImageUrl = house.ImageUrl
            };

            return View(houseModel);
        }

        [Authorize]
        public IActionResult Add()
        {
            if (!this.data.Agents.Any(a => a.UserId == this.User.Id()))
            {
                return RedirectToAction(nameof(AgentsController.Become), "Agents");
            }

            return View(new HouseFormModel
            {
                Categories = this.GetHouseCategories()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(HouseFormModel model)
        {
            if (!this.data.Agents.Any(a => a.UserId == this.User.Id()))
            {
                return RedirectToAction(nameof(AgentsController.Become), "Agents");
            }

            if (!this.data.Categories.Any(c => c.Id == model.CategoryId))
            {
                this.ModelState.AddModelError(nameof(model.CategoryId), "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = this.GetHouseCategories();

                return View(model);
            }

            var agentId = this.data.Agents
                .First(a => a.UserId == this.User.Id())
                .Id;

            var house = new House
            {
                Title = model.Title,
                Address = model.Address,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                PricePerMonth = model.PricePerMonth,
                CategoryId = model.CategoryId,
                AgentId = agentId
            };

            this.data.Houses.Add(house);
            this.data.SaveChanges();

            return RedirectToAction(nameof(Details), new { id = house.Id });
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var house = this.data.Houses.Find(id);

            if (house is null)
            {
                return BadRequest();
            }

            var agent = this.data.Agents.FirstOrDefault(a => a.Id == house.AgentId);

            if (agent?.UserId != this.User.Id())
            {
                return Unauthorized();
            }

            var houseModel = new HouseFormModel()
            {
                Title = house.Title,
                Address = house.Address,
                Description = house.Description,
                ImageUrl = house.ImageUrl,
                PricePerMonth = house.PricePerMonth,
                CategoryId = house.CategoryId,
                Categories = this.GetHouseCategories()
            };

            return View(houseModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(int id, HouseFormModel model)
        {
            var house = this.data.Houses.Find(id);
            if (house is null)
            {
                return this.View();
            }

            var agent = this.data.Agents.FirstOrDefault(a => a.Id == house.AgentId);

            if (agent?.UserId != this.User.Id())
            {
                return Unauthorized();
            }

            if (!this.data.Categories.Any(c => c.Id == model.CategoryId))
            {
                this.ModelState.AddModelError(nameof(model.CategoryId), "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = this.GetHouseCategories();

                return View(model);
            }

            house.Title = model.Title;
            house.Address = model.Address;
            house.Description = model.Description;
            house.ImageUrl = model.ImageUrl;
            house.PricePerMonth = model.PricePerMonth;
            house.CategoryId = model.CategoryId;

            this.data.SaveChanges();

            return RedirectToAction(nameof(Details), new { id = house.Id });
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var house = this.data.Houses.Find(id);

            if (house is null)
            {
                return BadRequest();
            }

            var agent = this.data.Agents.FirstOrDefault(a => a.Id == house.AgentId);

            if (agent?.UserId != this.User.Id())
            {
                return Unauthorized();
            }

            var model = new HouseViewModel()
            {
                Title = house.Title,
                Address = house.Address,
                ImageUrl = house.ImageUrl
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Delete(HouseViewModel model)
        {
            var house = this.data.Houses.Find(model.Id);

            if (house is null)
            {
                return BadRequest();
            }

            var agent = this.data.Agents.FirstOrDefault(a => a.Id == house.AgentId);

            if (agent?.UserId != this.User.Id())
            {
                return Unauthorized();
            }

            this.data.Houses.Remove(house);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        [Authorize]
        public IActionResult Rent(int id)
        {
            var house = this.data.Houses.Find(id);
            if (house is null || house.RenterId is not null)
            {
                return BadRequest();
            }

            if (this.data.Agents.Any(a => a.UserId == this.User.Id()))
            {
                return Unauthorized();
            }

            house.RenterId = this.User.Id();
            this.data.SaveChanges();

            return RedirectToAction(nameof(Mine));
        }

        [HttpPost]
        [Authorize]
        public IActionResult Leave(int id)
        {
            var house = this.data.Houses.Find(id);
            if (house is null || house.RenterId is null)
            {
                return BadRequest();
            }

            if (house.RenterId != this.User.Id())
            {
                return Unauthorized();
            }

            house.RenterId = null;
            this.data.SaveChanges();

            return RedirectToAction(nameof(Mine));
        }

        private IEnumerable<HouseCategoryViewModel> GetHouseCategories()
            => this.data
                .Categories
                .Select(c => new HouseCategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();
    }
}
