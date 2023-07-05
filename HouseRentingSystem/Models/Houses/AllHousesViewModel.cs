namespace HouseRentingSystem.Models.Houses
{
    public class AllHousesViewModel
    {
        public IEnumerable<HouseDetailsViewModel> Houses { get; set; }
            = new List<HouseDetailsViewModel>();
    }
}
