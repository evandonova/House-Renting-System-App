using AutoMapper;
using HouseRentingSystem.Data;
using HouseRentingSystem.Tests.Mocks;
using HouseRentingSystem.Data.Entities;

namespace HouseRentingSystem.Tests.UnitTests
{
    public class UnitTestsBase
    {
        protected HouseRentingDbContext data;
        protected IMapper mapper;

        [OneTimeSetUp]
        public void SetUpBase()
        {
            this.data = DatabaseMock.Instance;
            this.mapper = MapperMock.Instance;
            this.SeedDatabase();
        }

        public User Renter { get; private set; }
        public Agent Agent { get; private set; }
        public House RentedHouse { get; private set; }

        private void SeedDatabase()
        {
            this.Renter = new User()
            {
                Id = "RenterUserId",
                Email = "rent@er.bg",
                FirstName = "Renter",
                LastName = "User"
            };
            this.data.Users.Add(this.Renter);

            this.Agent = new Agent()
            {
                PhoneNumber = "+359111111111",
                User = new User()
                {
                    Id = "TestUserId",
                    Email = "test@test.bg",
                    FirstName = "Test",
                    LastName = "Tester"
                }
            };
            this.data.Agents.Add(this.Agent);

            this.RentedHouse = new House()
            {
                Title = "First Test House",
                Renter = this.Renter,
                Agent = this.Agent,
                Address = "Sofia, Bulgaria",
                ImageUrl = "image.png",
                Description = "Test rented house",
                Category = new Category() { Name = "Cottage" }
            };
            this.data.Houses.Add(this.RentedHouse);

            var nonRentedHouse = new House()
            {
                Title = "Second Test House",
                Address = "Varna, Bulgaria",
                ImageUrl = "image-2.png",
                Description = "Test non-rented house",
            };

            this.data.Houses.Add(nonRentedHouse);
            this.data.SaveChanges();
        }

        [OneTimeTearDown]
        public void TearDownBase()
           => this.data.Dispose();
    }
}
