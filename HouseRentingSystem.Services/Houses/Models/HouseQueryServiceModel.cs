namespace HouseRentingSystem.Services.Houses.Models
{
    public class HouseQueryServiceModel
    {
        public int TotalHousesCount { get; init; }

        public IEnumerable<HouseServiceModel> Houses { get; init; }
            = new List<HouseServiceModel>();
    }
}
