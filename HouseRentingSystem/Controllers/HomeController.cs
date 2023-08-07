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
            var totalHouses = this.data.Houses.Count();

            var totalRents = this.data.Houses
                .Where(h => h.RenterId != null).Count();

            var houses = this.data
                .Houses
                .OrderByDescending(c => c.Id)
                .Select(c => new HouseIndexViewModel
                {
                    Id = c.Id,
                    Title = c.Title,
                    ImageUrl = c.ImageUrl
                })
                .Take(3)
                .ToList();

            return View(new IndexViewModel
            {
                TotalHouses = totalHouses,
                TotalRents = totalRents,
                Houses = houses
            });
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