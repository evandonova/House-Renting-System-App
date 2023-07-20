using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using HouseRentingSystem.Data.Entities;

namespace HouseRentingSystem.Data
{
    public class HouseRentingDbContext : IdentityDbContext
    {
        private IdentityUser agentUser { get; set; } = null!;
        private IdentityUser guestUser { get; set; } = null!;
        private Agent agent { get; set; } = null!;
        private Category cottageCategory { get; set; } = null!;
        private Category singleCategory { get; set; } = null!;
        private Category duplexCategory { get; set; } = null!;
        private House firstHouse { get; set; } = null!;
        private House secondHouse { get; set; } = null!;
        private House thirdHouse { get; set; } = null!;

        public HouseRentingDbContext(DbContextOptions<HouseRentingDbContext> options)
            : base(options)
        {
        }

        public DbSet<House> Houses { get; init; } = null!;
        public DbSet<Category> Categories { get; init; } = null!;
        public DbSet<Agent> Agents { get; init; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<House>()
                .HasOne(h => h.Category)
                .WithMany(c => c.Houses)
                .HasForeignKey(h => h.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
               .Entity<House>()
               .HasOne(h => h.Agent)
               .WithMany(a => a.ManagedHouses)
               .HasForeignKey(h => h.AgentId)
               .OnDelete(DeleteBehavior.Restrict);

            SeedUsers();
            builder.Entity<IdentityUser>()
                    .HasData(this.agentUser, this.guestUser);

            SeedAgent();
            builder.Entity<Agent>()
                    .HasData(this.agent);

            SeedCategories();
            builder.Entity<Category>()
                .HasData(this.cottageCategory,
                        this.singleCategory,
                        this.duplexCategory);

            SeedHouses();
            builder.Entity<House>()
                .HasData(this.firstHouse,
                        this.secondHouse,
                        this.thirdHouse);

            base.OnModelCreating(builder);
        }

        private void SeedUsers()
        {
            var hasher = new PasswordHasher<IdentityUser>();

            this.agentUser = new IdentityUser()
            {
                Id = "dea12856-c198-4129-b3f3-b893d8395082",
                UserName = "agent@mail.com",
                NormalizedUserName = "agent@mail.com",
                Email = "agent@mail.com",
                NormalizedEmail = "agent@mail.com"
            };

            this.agentUser.PasswordHash =
              hasher.HashPassword(this.agentUser, "agent123");

            this.guestUser = new IdentityUser()
            {
                Id = "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                UserName = "guest@mail.com",
                NormalizedUserName = "guest@mail.com",
                Email = "guest@mail.com",
                NormalizedEmail = "guest@mail.com"
            };

            this.guestUser.PasswordHash =
              hasher.HashPassword(this.guestUser, "guest123");
        }

        private void SeedAgent()
        {
            this.agent = new Agent()
            {
                Id = Guid.Parse("44a41a1c-943b-47e2-80e6-47463b6f139b"),
                PhoneNumber = "+359888888888",
                UserId = this.agentUser.Id
            };
        }

        private void SeedCategories()
        {
            this.cottageCategory = new Category()
            {
                Id = 1,
                Name = "Cottage"
            };

            this.singleCategory = new Category()
            {
                Id = 2,
                Name = "Single-Family"
            };

            this.duplexCategory = new Category()
            {
                Id = 3,
                Name = "Duplex"
            };
        }

        private void SeedHouses()
        {
            this.firstHouse = new House()
            {
                Id = 1,
                Title = "Big House Marina",
                Address = "North London, UK (near the border)",
                Description = "A big house for your whole family. Don't miss to buy a house with three bedrooms.",
                ImageUrl = "https://www.luxury-architecture.net/wp-content/uploads/2017/12/1513217889-7597-FAIRWAYS-010.jpg",
                PricePerMonth = 2100.00M,
                CategoryId = this.duplexCategory.Id,
                AgentId = this.agent.Id,
                RenterId = this.guestUser.Id
            };

            this.secondHouse = new House()
            {
                Id = 2,
                Title = "Family House Comfort",
                Address = "Near the Sea Garden in Burgas, Bulgaria",
                Description = "It has the best comfort you will ever ask for. With two bedrooms, it is great for your family.",
                ImageUrl = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/179489660.jpg?k=2029f6d9589b49c95dcc9503a265e292c2cdfcb5277487a0050397c3f8dd545a&o=&hp=1",
                PricePerMonth = 1200.00M,
                CategoryId = this.singleCategory.Id,
                AgentId = this.agent.Id
            };

            this.thirdHouse = new House()
            {
                Id = 3,
                Title = "Grand House",
                Address = "Boyana Neighbourhood, Sofia, Bulgaria",
                Description = "This luxurious house is everything you will need. It is just excellent.",
                ImageUrl = "https://i.pinimg.com/originals/a6/f5/85/a6f5850a77633c56e4e4ac4f867e3c00.jpg",
                PricePerMonth = 2000.00M,
                CategoryId = this.singleCategory.Id,
                AgentId = this.agent.Id
            };
        }
    }
}