using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Kenny.Services.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Kenny.Services.Identity.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ProfileService(IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            string sub = context.Subject.GetSubjectId();
            ApplicationUser user = await _userManager.FindByIdAsync(sub);
            ClaimsPrincipal userClaims = await _userClaimsPrincipalFactory.CreateAsync(user);

            List<Claim> claimsList = userClaims.Claims.ToList();
            claimsList = claimsList.Where(c => context.RequestedClaimTypes.Contains(c.Type)).ToList();

            if (_userManager.SupportsUserRole)
            {
                IList<string> roles = await _userManager.GetRolesAsync(user);
                foreach(var rolename in roles)
                {
                    claimsList.Add(new Claim(JwtClaimTypes.Role, rolename));
                    claimsList.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));
                    claimsList.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));
                    if (_userManager.SupportsUserRole)
                    {
                        IList<string> rolesList = await _userManager.GetRolesAsync(user);
                        foreach(var roleName in rolesList)
                        {
                            claimsList.Add(new Claim(JwtClaimTypes.Role, rolename));
                            if (_roleManager.SupportsRoleClaims)
                            {
                                IdentityRole role = await _roleManager.FindByNameAsync(roleName);
                                if(role != null)
                                {
                                    claimsList.AddRange(await _roleManager.GetClaimsAsync(role));
                                }
                            }
                        }
                    }
                }
            }

            context.IssuedClaims = claimsList;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            string sub = context.Subject.GetSubjectId();
            ApplicationUser user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}

