using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HouseRentingSystem.Models;
using HouseRentingSystem.Data;
using HouseRentingSystem.Models.Home;

namespace HouseRentingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly HouseRentingDbContext data;

        public HomeController(HouseRentingDbContext data)
            => this.data = data;

        public IActionResult Index()
        {
            var allHouses = new IndexViewModel()
            {
                TotalHouses = this.data.Houses.Count(),
                TotalRents = this.data.Houses.Where(h => h.RenterId != null).Count(),
                Houses = this.data.Houses
                    .Select(h => new HouseIndexViewModel()
                    {
                        Title = h.Title,
                        ImageUrl = h.ImageUrl
                    })
            };

            return View(allHouses);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? 
                HttpContext.TraceIdentifier });
        }
    }
}