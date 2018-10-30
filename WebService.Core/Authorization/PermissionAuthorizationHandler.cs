using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebService.Core.Authorization
{

    public class PermissionAuthorizationRequirement : IAuthorizationRequirement
    {
        public PermissionAuthorizationRequirement(Operation operation)
        {
            Operation = operation;
        }

        public Operation Operation { get; set; }
    }
    public class PermissionAuthorizationHandler<T> : AuthorizationHandler<PermissionAuthorizationRequirement> where T : IdentityUser<Guid>
    {
        private readonly UserManager<T> _userManager;
        public PermissionAuthorizationHandler(UserManager<T> userManager)
        {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
        {
            if (context.User != null)
            {
                if (context.User.IsInRole("admin"))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    var userIdClaim = context.User.FindFirst(_ => _.Type == ClaimTypes.NameIdentifier);
                    if (userIdClaim != null)
                    {
                        //if (_userStore.CheckPermission(int.Parse(userIdClaim.Value), requirement.Name))
                        //{
                        //    context.Succeed(requirement);
                        //}
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}
