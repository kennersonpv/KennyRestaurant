using Microsoft.EntityFrameworkCore;

namespace Kenny.Services.CouponAPI.DbContexts
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}
	}
}
