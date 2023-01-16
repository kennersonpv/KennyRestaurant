using Microsoft.EntityFrameworkCore;

namespace Kenny.Services.ShoppingCartAPI.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        //public DbSet<Product> Products { get; set; }
    }
}
