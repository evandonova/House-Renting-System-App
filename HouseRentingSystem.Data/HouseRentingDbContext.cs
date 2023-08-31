using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using HouseRentingSystem.Data.Entities;

using static HouseRentingSystem.Data.Constants;

namespace HouseRentingSystem.Data
{
    public class HouseRentingDbContext : IdentityDbContext<User>
    {
        private bool seedDb = true;
        private User agentUser { get; set; } = null!;
        private User guestUser { get; set; } = null!;
        private User adminUser { get; set; } = null!;
        private Agent agent { get; set; } = null!;
        private Agent adminAgent { get; set; } = null!;
        private Category cottageCategory { get; set; } = null!;
        private Category singleCategory { get; set; } = null!;
        private Category duplexCategory { get; set; } = null!;
        private House firstHouse { get; set; } = null!;
        private House secondHouse { get; set; } = null!;
        private House thirdHouse { get; set; } = null!;

        public HouseRentingDbContext
            (DbContextOptions<HouseRentingDbContext> options, bool seedDb = true)
            : base(options)
        {
            if (!this.Database.IsRelational())
            {
                this.Database.EnsureCreated();
            }
            else
            {
                this.Database.Migrate();
            }

            this.seedDb = seedDb;
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

            if(this.seedDb)
            {
                SeedUsers();
                builder.Entity<User>()
                        .HasData(this.agentUser,
                        this.guestUser,
                        this.adminUser);

                SeedAgent();
                builder.Entity<Agent>()
                        .HasData(this.agent,
                        this.adminAgent);

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
            }

            base.OnModelCreating(builder);
        }

        private void SeedUsers()
        {
            var hasher = new PasswordHasher<User>();

            this.agentUser = new User()
            {
                Id = "dea12856-c198-4129-b3f3-b893d8395082",
                UserName = "agent@mail.com",
                NormalizedUserName = "AGENT@MAIL.COM",
                Email = "agent@mail.com",
                NormalizedEmail = "AGENT@MAIL.COM",
                FirstName = "Linda",
                LastName = "Michaels"
            };

            this.agentUser.PasswordHash =
              hasher.HashPassword(this.agentUser, "agent123");

            this.guestUser = new User()
            {
                Id = "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                UserName = "guest@mail.com",
                NormalizedUserName = "GUEST@MAIL.COM",
                Email = "guest@mail.com",
                NormalizedEmail = "GUEST@MAIL.COM",
                FirstName = "Teodor",
                LastName = "Lesly"
            };

            this.guestUser.PasswordHash =
              hasher.HashPassword(this.guestUser, "guest123");

            this.adminUser = new User()
            {
                Id = "787345d0-d1a4-416f-8f8c-e6d40b96c0b3",
                UserName = AdminEmail,
                NormalizedUserName = AdminEmail.ToUpper(),
                Email = AdminEmail,
                NormalizedEmail = AdminEmail.ToUpper(),
                FirstName = "Great",
                LastName = "Admin"
            };

            this.adminUser.PasswordHash =
              hasher.HashPassword(this.adminUser, "admin123");
        }

        private void SeedAgent()
        {
            this.agent = new Agent()
            {
                Id = Guid.Parse("44a41a1c-943b-47e2-80e6-47463b6f139b"),
                PhoneNumber = "+359888888888",
                UserId = this.agentUser.Id
            };

            this.adminAgent = new Agent()
            {
                Id = Guid.Parse("2d0b01e8-07fc-4069-80c3-bae95b27ff53"),
                PhoneNumber = "+359123456789",
                UserId = this.adminUser.Id
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