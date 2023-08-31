using HouseRentingSystem.Data.Entities;
using HouseRentingSystem.Services.Houses;
using HouseRentingSystem.Services.Users;

namespace HouseRentingSystem.Tests.UnitTests
{
    [TestFixture]
    public class HouseServiceTests : UnitTestsBase
    {
        private IUserService userService;
        private IHouseService houseService;

        [OneTimeSetUp]
        public void SetUp()
        {
            this.userService = new UserService(this.data, this.mapper);
            this.houseService = new HouseService(this.data, this.userService, this.mapper);
        }

        [Test]
        public async Task All_ShouldReturnCorrectHouses()
        {
            // Arrange: create a variable for the search term
            var searchTerm = "First";

            // Act: invoke the service method with the term
            var result = await this.houseService.AllAsync(null, searchTerm);

            // Assert the returned houses' count is correct
            var housesInDb = this.data.Houses
                .Where(h => h.Title.Contains(searchTerm));
            Assert.That(result.TotalHousesCount, Is.EqualTo(housesInDb.Count()));

            // Assert a returned house data is correct
            var resultHouse = result.Houses.FirstOrDefault();
            Assert.IsNotNull(result);

            var houseInDb = housesInDb.FirstOrDefault();
            Assert.That(resultHouse?.Id, Is.EqualTo(houseInDb?.Id));
            Assert.That(resultHouse.Title, Is.EqualTo(houseInDb?.Title));
        }

        [Test]
        public async Task AllCategoryNames_ShouldReturnCorrectResult()
        {
            // Arrange

            // Act: invoke the service method
            var result = await this.houseService.AllCategoriesNamesAsync();

            // Assert the returned categories' count is correct
            var dbCategories = this.data.Categories;
            Assert.That(result.Count(), Is.EqualTo(dbCategories.Count()));

            // Assert the returned categories' are correct
            var categoryNames = dbCategories.Select(c => c.Name);
            Assert.That(categoryNames.Contains(result.FirstOrDefault()));
        }

        [Test]
        public async Task AllHousesByAgentId_ShouldReturnCorrectHouses()
        {
            // Arrange: add a valid agent id to a variable
            var agentId = this.Agent.Id;

            // Act: invoke the service method with valid agent id
            var result = await this.houseService.AllHousesByAgentIdAsync(agentId.ToString());

            // Assert the returned result is not null
            Assert.IsNotNull(result);

            // Assert the returned houses' count is correct
            var housesInDb = this.data.Houses
                .Where(h => h.AgentId == agentId);
            Assert.That(result.Count(), Is.EqualTo(housesInDb.Count()));
        }

        [Test]
        public async Task AllHousesByUserId_ShouldReturnCorrectHouses()
        {
            // Arrange: add a valid renter id to a variable
            var renterId = this.Renter.Id;

            // Act: invoke the service method with valid renter id
            var result = await this.houseService.AllHousesByUserIdAsync(renterId);

            // Assert the returned result is not null
            Assert.IsNotNull(result);

            // Assert the returned houses' count is correct
            var housesInDb = this.data.Houses
                .Where(h => h.RenterId == renterId);
            Assert.That(result.Count(), Is.EqualTo(housesInDb.Count()));
        }

        [Test]
        public async Task Exists_ShouldReturnCorrectTrue_WithValidId()
        {
            // Arrange: get a valid rented house id
            var houseId = this.RentedHouse.Id;

            // Act: invoke the service method with the valid id
            var result = await this.houseService.ExistsAsync(houseId);

            // Assert the returned result is true
            Assert.IsTrue(result);
        }

        [Test]
        public async Task HouseDetailsById_ShouldReturnCorrectHouseData()
        {
            // Arrange: get a valid rented house id
            var houseId = this.RentedHouse.Id;

            // Act: invoke the service method with the valid id
            var result = await this.houseService.HouseDetailsByIdAsync(houseId);

            // Assert the returned result is not null
            Assert.IsNotNull(result);

            // Assert the returned result data is correct
            var houseInDb = this.data.Houses.Find(houseId);
            Assert.That(result.Id, Is.EqualTo(houseInDb?.Id));
            Assert.That(result.Title, Is.EqualTo(houseInDb?.Title));
        }

        [Test]
        public async Task AllCategories_ShouldReturnCorrectCategories()
        {
            // Arrange

            // Act: invoke the service method
            var result = await this.houseService.AllCategoriesAsync();

            // Assert the returned categories' count is correct
            var dbCategories = this.data.Categories;
            Assert.That(result.Count(), Is.EqualTo(dbCategories.Count()));

            // Assert the returned categories are correct
            var categoryNames = dbCategories.Select(c => c.Name);
            Assert.That(categoryNames.Contains(result.FirstOrDefault()?.Name));
        }

        [Test]
        public async Task CategoryExists_ShouldReturnTrue_WithValidId()
        {
            // Arrange: get a valid category id
            var categoryId = this.data.Categories.First().Id;

            // Act: invoke the service method with the valid id
            var result = await this.houseService.CategoryExistsAsync(categoryId);

            // Assert the returned result is true
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Create_ShouldCreateHouse()
        {
            // Arrange: get the houses current count
            var housesInDbBefore = this.data.Houses.Count();

            // Arrange: create a new House variable with needed data
            var newHouse = new House()
            {
                Title = "New House"
            };

            // Act: invoke the service method with neccessary valid data
            var newHouseId = await this.houseService.CreateAsync(newHouse.Title, "Germany", 
                "Best house", string.Empty, 2200.00M, 1, this.Agent.Id.ToString());

            // Assert the houses' current count has increased by 1
            var housesInDbAfter = this.data.Houses.Count();
            Assert.That(housesInDbAfter, Is.EqualTo(housesInDbBefore + 1));

            // Assert the new house is created with correct data
            var newHouseInDb = this.data.Houses.Find(newHouseId);
            Assert.That(newHouseInDb?.Title, Is.EqualTo(newHouse.Title));
        }

        [Test]
        public async Task HasAgentWithId_ShouldReturnTrue_WithValidId()
        {
            // Arrange: get valid rented house's renter and agent ids
            var houseId = this.RentedHouse.Id;
            var userId = this.RentedHouse.Agent.User.Id;

            // Act: invoke the service method with valid ids
            var result = await this.houseService.HasAgentWithIdAsync(houseId, userId);

            // Assert the returned result is true
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetHouseCategoryId_ShouldReturnCorrectId()
        {
            // Arrange: get valid rented house's renter id
            var houseId = this.RentedHouse.Id;

            // Act: invoke the service method with valid id
            var result = await this.houseService.GetHouseCategoryIdAsync(houseId);

            // Assert the returned result is not null
            Assert.IsNotNull(result);

            // Assert the returned category is correct
            var categoryId = this.RentedHouse.Category.Id;
            Assert.That(result, Is.EqualTo(categoryId));
        }

        [Test]
        public void Edit_ShouldEditHouseCorrectly()
        {
            // Arrange: add a new house to the database
            var house = new House()
            {
                Title = "New House for Edit",
                Address = "Sofia",
                Description = "Edit",
                ImageUrl = string.Empty
            };

            this.data.Houses.Add(house);
            this.data.SaveChanges();

            // Arrange: create a variable with the changed address
            var changedAddress = "Sofia, Bulgaria";

            // Act: invoke the method with valid data and changed address
            this.houseService.EditAsync(house.Id, house.Title, changedAddress,
                house.Description, house.ImageUrl, house.PricePerMonth, 
                house.CategoryId);

            // Assert the house data in the database is correct
            var newHouseInDb = this.data.Houses.Find(house.Id);
            Assert.IsNotNull(newHouseInDb);
            Assert.That(newHouseInDb.Title, Is.EqualTo(house.Title));
            Assert.That(newHouseInDb.Address, Is.EqualTo(changedAddress));
        }

        [Test]
        public void Delete_ShouldDeleteHouseSuccessfully()
        {
            // Arrange: add a new house to the database
            var house = new House()
            {
                Title = "New House for delete",
                Address = "Sofia",
                Description = "Delete",
                ImageUrl = string.Empty
            };

            this.data.Houses.Add(house);
            this.data.SaveChanges();

            // Arrange: get the current houses' count
            var housesCountBefore = this.data.Houses.Count();

            // Act: invoke the service method with valid id
            this.houseService.DeleteAsync(house.Id);

            // Assert the returned houses' cound has decreased by 1
            var housesCountAfter = this.data.Houses.Count();
            Assert.That(housesCountAfter, Is.EqualTo(housesCountBefore - 1));

            // Assert the house is not present in the db
            var houseInDb = this.data.Houses.Find(house.Id);
            Assert.IsNull(houseInDb);
        }

        [Test]
        public async Task IsRented_ShouldReturnCorrectTrue_WithValidId()
        {
            // Arrange: get a valid rented house id
            var houseId = this.RentedHouse.Id;

            // Act: invoke the service method with valid id
            var result = await this.houseService.IsRentedAsync(houseId);

            // Assert the returned result is true
            Assert.IsTrue(result);
        }

        [Test]
        public async Task IsRentedByUserWithId_ShouldReturnCorrectTrue_WithValidId()
        {
            // Arrange: get valid rented house and renter ids
            var houseId = this.RentedHouse.Id;
            var renterId = this.RentedHouse.Renter!.Id;

            // Act: invoke the service method with valid ids
            var result = await this.houseService
                .IsRentedByUserWithIdAsync(houseId, renterId);

            // Assert the returned result is true
            Assert.IsTrue(result);
        }

        [Test]
        public void Rent_ShouldRentHouseSuccessfully()
        {
            // Arrange: add a new house to the db
            var house = new House()
            {
                Title = "New House for rent",
                Address = "Germany",
                Description = "Rent",
                ImageUrl = string.Empty
            };

            this.data.Houses.Add(house);
            this.data.SaveChanges();

            // Arrange: get a valid renter id
            var renterId = this.Renter.Id;

            // Act: invoke the service method with valid ids
            this.houseService.RentAsync(house.Id, renterId);

            // Assert the house has correct data in the db
            var newHouseInDb = this.data.Houses.Find(house.Id);
            Assert.IsNotNull(newHouseInDb);
            Assert.That(renterId, Is.EqualTo(house.RenterId));
        }

        [Test]
        public void Leave_ShouldLeaveHouseSuccessfully()
        {
            // Arrange: add a new house to the db
            var house = new House()
            {
                Title = "New House for leave",
                RenterId = "TestRenterId",
                Address = "Germany",
                Description = "Leave",
                ImageUrl = string.Empty
            };

            this.data.Houses.Add(house);
            this.data.SaveChanges();

            // Act: invoke the service method with valid id
            this.houseService.LeaveAsync(house.Id);

            // Assert the house has correct data in the db
            var newHouseInDb = this.data.Houses.Find(house.Id);
            Assert.IsNotNull(newHouseInDb);
            Assert.IsNull(newHouseInDb.RenterId);
        }

        [Test]
        public async Task LastThreeHouses_ShouldReturnCorrectHouses()
        {
            // Arrange

            // Act: invoke the service method
            var result = await this.houseService.LastThreeHousesAsync();

            // Assert the retuned houses count is correct
            var housesInDb = this.data.Houses
                .OrderByDescending(h => h.Id)
                .Take(3);
            Assert.That(result.Count(), Is.EqualTo(housesInDb.Count()));

            // Assert a retuned house's data is correct
            var firstHouseInDb = housesInDb
                .FirstOrDefault();

            var firstResultHouse = result.FirstOrDefault();
            Assert.That(firstResultHouse?.Id, Is.EqualTo(firstHouseInDb?.Id));
            Assert.That(firstResultHouse.Title, Is.EqualTo(firstHouseInDb?.Title));
        }
    }
}
