using System.Collections.Generic;

namespace HouseRentingSystem.Models.Home
{
    public class IndexViewModel
    {
        public int TotalHouses { get; init; }

        public int TotalRents { get; init; }

        public IEnumerable<HouseIndexViewModel> Houses { get; init; } 
            = new List<HouseIndexViewModel>();
    }
}