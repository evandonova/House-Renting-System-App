using Microsoft.AspNetCore.Mvc;
using HouseRentingSystem.Services.Houses;

namespace HouseRentingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHouseService houses;

        public HomeController(IHouseService houses)
            => this.houses = houses;

        public IActionResult Index()
        {
            var houses = this.houses.LastThreeHouses();
            return View(houses);
        }

        public IActionResult Error(int statusCode)
        {
            if(statusCode == 400 || statusCode == 404)
            {
                return View("Error400");
            }

            if(statusCode == 401)
            {
                return View("Error401");
            }

            return View();
        }
    }
}