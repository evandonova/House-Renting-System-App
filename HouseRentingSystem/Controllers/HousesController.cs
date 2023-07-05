using Microsoft.AspNetCore.Mvc;
using HouseRentingSystem.Models.Houses;

namespace HouseRentingSystem.Controllers
{
    public class HousesController : Controller
    {
        public IActionResult All()
        {
            var allHouses = new AllHousesViewModel()
            {
                Houses = Common.GetHouses()
            };

            return View(allHouses);
        }

        public IActionResult Details()
        {
            var house = Common.GetHouses().FirstOrDefault();

            return View(house);
        }
    }
}
