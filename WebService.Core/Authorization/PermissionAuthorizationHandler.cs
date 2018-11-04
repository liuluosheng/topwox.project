using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using EnumsNET;
namespace WebService.Core.Authorization
{

    public class PermissionAuthorizationRequirement : IAuthorizationRequirement
    {
        public PermissionAuthorizationRequirement(Operation operation, Type entityType = null)
        {
            Operation = operation;
            EntityType = entityType;
        }

        public Operation Operation { get; set; }
        public Type EntityType { get; set; }
    }
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                if (context.User.IsInRole("admin"))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    foreach (var operation in requirement.Operation.GetFlags())
                    {
                        string op = operation.ToString();
                        if (requirement.EntityType != null && operation.GetAttributes().Any(p => p is PublicAttribute))
                        {
                            op = $"{requirement.EntityType.Name}_{op}";
                        }
                        if (!context.User.Claims.Any(p => p.Type.ToLower() == "operation" && p.Value == op))
                        {
                            context.Fail();
                            break;
                        }
                    }
                    if (!context.HasFailed)
                    {
                        context.Succeed(requirement);
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}
