using Duende.IdentityServer.Models;

namespace Kenny.Services.Identity
{
	//static details
	public static class SD
	{
		public const string Admin = "Admin";
		public const string Customer = "Customer";

		public static IEnumerable<IdentityResource> IdentityResources =>
			new List<IdentityResource>
			{
				new IdentityResources.OpenId(),
				new IdentityResources.Email(),
				new IdentityResources.Profile()
			};

		public static IEnumerable<ApiScope> ApiScopes =>
			new List<ApiScope>
			{
				new ApiScope("KennyAdmin", "Kenny Server"),
				new ApiScope(name: "read", displayName:"Read data"),
				new ApiScope(name: "write", displayName:"Write data"),
				new ApiScope(name: "delete", displayName:"Delete data"),
			};

		public static IEnumerable<Client> Clients =>
			new List<Client>
			{
				new Client
				{
					ClientId="client",
					ClientSecrets={new Secret("secret".Sha256())},
					AllowedGrantTypes = GrantTypes.ClientCredentials,
					AllowedScopes={"read", "write", "profile"}
				}
			};
	}
}
