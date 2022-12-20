using Microsoft.AspNetCore.Identity;

namespace Kenny.Services.Identity.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}
}
