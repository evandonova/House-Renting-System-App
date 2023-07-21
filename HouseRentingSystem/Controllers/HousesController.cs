using Microsoft.AspNetCore.Mvc;
using HouseRentingSystem.Models.Houses;
using HouseRentingSystem.Data;
using HouseRentingSystem.Models.Home;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HouseRentingSystem.Controllers
{
    public class HousesController : Controller
    {
        private readonly HouseRentingDbContext data;

        public HousesController(HouseRentingDbContext data)
            => this.data = data;

        public IActionResult All()
        {
            var allHouses = new AllHousesViewModel()
            {
                Houses = this.data.Houses
                    .Select(h => new HouseDetailsViewModel 
                    { 
                        Title = h.Title,
                        Address = h.Address,
                        ImageUrl = h.ImageUrl
                    })
            };

            return View(allHouses);
        }

        [Authorize]
        public IActionResult Mine()
        {
            var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var allHouses = new AllHousesViewModel()
            {
                Houses = this.data.Houses
                    .Where(h => h.Agent.UserId == currentUserId)
                    .Select(h => new HouseDetailsViewModel()
                    {
                        Title = h.Title,
                        Address = h.Address,
                        ImageUrl = h.ImageUrl
                    })
            };

            return View(allHouses);
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
    }
}
