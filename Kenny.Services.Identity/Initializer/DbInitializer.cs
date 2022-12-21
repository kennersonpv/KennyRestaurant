using IdentityModel;
using Kenny.Services.Identity.DbContexts;
using Kenny.Services.Identity.Initializer.Interfaces;
using Kenny.Services.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Kenny.Services.Identity.Initializer
{
	public class DbInitializer : IDbInitializer
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

		public void Initialize()
		{
			if(_roleManager.FindByNameAsync(SD.Admin).Result == null)
			{
				_roleManager.CreateAsync(new IdentityRole(SD.Admin)).GetAwaiter().GetResult();
				_roleManager.CreateAsync(new IdentityRole(SD.Customer)).GetAwaiter().GetResult();
			}
			else { return; }

			ApplicationUser adminUser = new ApplicationUser()
			{
				UserName = "kennersonpv@gmail.com",
				Email = "kennersonpv@gmail.com",
				EmailConfirmed = true,
				PhoneNumber = "31999999999",
				FirstName = "Kennerson",
				LastName = "Vitor"
			};

			_userManager.CreateAsync(adminUser, "Admin123*").GetAwaiter().GetResult();
			_userManager.AddToRoleAsync(adminUser,SD.Admin).GetAwaiter().GetResult();

			var adminUserClaims = _userManager.AddClaimsAsync(adminUser, new Claim[ ]
			{
				new Claim(JwtClaimTypes.Name, adminUser.FirstName+" "+adminUser.LastName),
				new Claim(JwtClaimTypes.GivenName, adminUser.FirstName),
				new Claim(JwtClaimTypes.FamilyName, adminUser.LastName),
				new Claim(JwtClaimTypes.Role, SD.Admin),
			}).Result;

			ApplicationUser customerUser = new ApplicationUser()
			{
				UserName = "customer@gmail.com",
				Email = "customer@gmail.com",
				EmailConfirmed = true,
				PhoneNumber = "31999999999",
				FirstName = "Customer",
				LastName = "User"
			};

			_userManager.CreateAsync(customerUser, "Admin123*").GetAwaiter().GetResult();
			_userManager.AddToRoleAsync(customerUser, SD.Customer).GetAwaiter().GetResult();

			var customerUserClaims = _userManager.AddClaimsAsync(customerUser, new Claim[]
			{
				new Claim(JwtClaimTypes.Name, customerUser.FirstName+" "+customerUser.LastName),
				new Claim(JwtClaimTypes.GivenName, customerUser.FirstName),
				new Claim(JwtClaimTypes.FamilyName, customerUser.LastName),
				new Claim(JwtClaimTypes.Role, SD.Customer),
			}).Result;
		}
	}
}
