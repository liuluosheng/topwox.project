using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
namespace IdentityServer
{
    public class CustomProfileService<T,R> : IProfileService 
        where T : IdentityUser<Guid> 
        where R:IdentityRole<Guid>
    {
        /// <summary>
        /// The logger
        /// </summary>
        protected readonly ILogger Logger;

        private readonly UserManager<T> _userManager;
        private readonly RoleManager<R> _roleManager;
        /// <summary>
        /// Initializes a new instance of the <see cref="TestUserProfileService"/> class.
        /// </summary>
        /// <param name="users">The users.</param>
        /// <param name="logger">The logger.</param>
        public CustomProfileService(UserManager<T> userManager, RoleManager<R> roleManager, ILogger<TestUserProfileService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            Logger = logger;
        }

        /// <summary>
        /// 只要有关用户的身份信息单元被请求（例如在令牌创建期间或通过用户信息终点），就会调用此方法
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public virtual async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.LogProfileRequest(Logger);

            //判断是否有请求Claim信息
            if (context.RequestedClaimTypes.Any())
            {
                //根据用户唯一标识查找用户信息
                var user = await _userManager.FindByIdAsync(context.Subject.GetSubjectId());
                if (user != null)
                {
                    //调用此方法以后内部会进行过滤，只将用户请求的Claim加入到 context.IssuedClaims 集合中 这样我们的请求方便能正常获取到所需Claim
                    var claims = (await _userManager.GetClaimsAsync(user)).ToList();
                    var roles = await _userManager.GetRolesAsync(user);
                    foreach (var name in roles)
                    {
                        var role = await _roleManager.FindByNameAsync(name);
                        if (role != null)
                            claims.AddRange(await _roleManager.GetClaimsAsync(role));
                    }
                    context.IssuedClaims.AddRange(claims);
                }
            }

            context.LogIssuedClaims(Logger);
        }



        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}