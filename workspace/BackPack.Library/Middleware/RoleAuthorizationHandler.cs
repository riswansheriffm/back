
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using BackPack.Dependency.Library.Helpers;

namespace BackPack.Library.Middleware
{
    public class RoleAuthorizationHandler(IConfiguration configuration) : AuthorizationHandler<RolesAuthorizationRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RolesAuthorizationRequirement requirement)
        {
            Aes256Helper aes256Helper = new(configuration);
            Claim claim = context.User.Claims.FirstOrDefault(option => option.Type == "UserRole")!;
            if (claim is not null)
            {
                var roles = requirement.AllowedRoles;
                List<string> roleList = [];
                string userRole = aes256Helper.Aes256Decryption(claim.Value);
                roleList.Add(userRole);

                if (roleList.Any(role => roles.Contains(role)))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                    return;
                }                
            }
            await Task.CompletedTask;
        }
    }
}
