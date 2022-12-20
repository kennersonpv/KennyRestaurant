using Kenny.Services.Identity.DbContexts;
using Kenny.Services.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Kenny.Services.Identity.Initializer
{
	public class DbInitializer : IDbSetInitializer
	{
		private readonly ApplicationDbContext _db;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public DbInitializer(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_db = db;
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public void InitializeSets(DbContext context)
		{
			if(_roleManager.FindByNameAsync(SD.Admin).Result == null)
			{
				_roleManager.CreateAsync(new IdentityRole(SD.Admin)).GetAwaiter().GetResult();
				_roleManager.CreateAsync(new IdentityRole(SD.Customer)).GetAwaiter().GetResult();
			}
			else { return; }

			ApplicationUser adminUser = new ApplicationUser()
			{
				UserName = "kennersonpv",
				Email = "kennersonpv@gmail.com",
				EmailConfirmed = true,
				PhoneNumber = "31999999999"
			};
		}
	}
}
