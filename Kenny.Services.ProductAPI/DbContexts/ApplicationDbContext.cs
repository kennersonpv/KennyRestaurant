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
                ImageUrl = "https://assets.unileversolutions.com/recipes-v2/54349.jpg",
                CategoryName = "Meal"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 2,
                Name = "Cheesburguer",
                Price = 29.99,
                Description = "It's our best seller. Picanha meat with cheddar cheese and a lot of bacon",
                ImageUrl = "https://s2.glbimg.com/jJirZVMNK5ZsZ9UDLKQBqPu3iXk=/620x455/e.glbimg.com/og/ed/f/original/2020/10/20/hamburgueria_bob_beef_-_dia_das_criancas_-_foto_pfz_studio__norma_lima.jpg",
                CategoryName = "Sandwich"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 3,
                Name = "Brigadeiro",
                Price = 10.99,
                Description = "Chocolate balls with grainy chocolate",
                ImageUrl = "https://img.itdg.com.br/tdg/images/recipes/000/000/114/201447/201447_original.jpg",
                CategoryName = "Dessert"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 4,
                Name = "Picanha",
                Price = 45,
                Description = "Juicy meet with rice and fries",
                ImageUrl = "https://marettimo.com.br/blog/wp-content/uploads/2022/10/Como-identificar-a-carne-que-parece-picanha.jpg",
                CategoryName = "Meal"
            });
        }
    }
}
