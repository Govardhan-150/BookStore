using Microsoft.EntityFrameworkCore;
using BookStore.Models;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace OnlineBookStoreApplication.Data
{
    public class ApplicationDbContext:IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CoverType> CoverTypes { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<NewShoppingCartList> NewShoppingCartLists { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<OrderHeader> OrederHeader { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductType>().HasData(
                new ProductType
                {
                    Id = 1,
                    Title = "Fortune of Time",
                    Author = "Billy Spark",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "SWD9999001",
                    ListPrice = 99,
                    Price = 90,
                    ListPrice50 = 85,
                    ListPrice100 = 80,
                    CategoryId = 2,
                    ImageUrl= "C:\\Users\\BusapalG\\Downloads\\course_9 (2).zip\\1 5 Bulky - MVC\\images\\fortune.jpg"
                },
                new ProductType
                {
                    Id = 2,
                    Title = "Dark Skies",
                    Author = "Nancy Hoover",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "CAW777777701",
                    ListPrice = 40,
                    Price = 30,
                    ListPrice50 = 25,
                    ListPrice100 = 20,
                    ImageUrl=""
                }
                );
            modelBuilder.Entity<Company>().HasData(
                new Company
                {
                    Id=1,
                    Name="Wipro",
                    City="Banglore",
                    StreetAddress="#27,EC2,BAnglore",
                    PostalCode="515201",
                    State="Karnataka",
                    PhoneNumber="9505341521"
                }
                );
        }

        }
    }
