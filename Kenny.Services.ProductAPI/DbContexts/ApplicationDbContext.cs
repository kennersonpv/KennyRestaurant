using Kenny.Services.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Kenny.Services.ProductAPI.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 1,
                Name = "Feijoada",
                Price = 15,
                Description = "Black beans with meat and rice",
                ImageUrl = "",
                CategoryName = "Meal"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 2,
                Name = "Cheesburguer",
                Price = 29.99,
                Description = "It's our best seller. Picanha meat with cheddar cheese and a lot of bacon",
                ImageUrl = "",
                CategoryName = "Sandwich"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 3,
                Name = "Brigadeiro",
                Price = 10.99,
                Description = "Chocolate balls with grainy chocolate",
                ImageUrl = "",
                CategoryName = "Dessert"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 4,
                Name = "Picanha",
                Price = 45,
                Description = "Juicy meet with rice and fries",
                ImageUrl = "",
                CategoryName = "Meal"
            });
        }
    }
}
