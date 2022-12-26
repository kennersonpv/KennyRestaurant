using Duende.IdentityServer;
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
				new ApiScope("kenny", "Kenny Server"),
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
				},
				new Client
				{
					ClientId="kenny",
					ClientSecrets={new Secret("secret".Sha256())},
					AllowedGrantTypes = GrantTypes.Code,
					RedirectUris={ "https://localhost:7294/signin-oidc" },
					PostLogoutRedirectUris={ "https://localhost:7294/signout-callback-oidc" },
					AllowedScopes= new List<string>
					{
					IdentityServerConstants.StandardScopes.OpenId,
					IdentityServerConstants.StandardScopes.Profile,
					IdentityServerConstants.StandardScopes.Email,
					"kenny"
					}
				}
			};
	}
}
