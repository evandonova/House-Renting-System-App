﻿using System.ComponentModel.DataAnnotations;

namespace HouseRentingSystem.Models.Houses
{
    public class AllHousesQueryModel
    {
        public const int HousesPerPage = 3;

        public string Category { get; init; } = null!;

        [Display(Name = "Search by text")]
        public string SearchTerm { get; init; } = null!;

        public HouseSorting Sorting { get; init; }

        public int CurrentPage { get; init; } = 1;

        public int TotalHousesCount { get; set; }

        public IEnumerable<string> Categories { get; set; } = new List<string>();

        public IEnumerable<HouseViewModel> Houses { get; set; } = new List<HouseViewModel>();
    }
}
