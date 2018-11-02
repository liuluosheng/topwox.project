using Core.IServices;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Core.Repository;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using WebService.Core.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace WebService.Core
{
    public static class StartupServiceConfig
    {

        public static void Config(IServiceCollection services, IConfiguration configuration)
        {    
            services.AddAuthorization(options =>
            {
                //options.AddPolicy(Operation.SystemMenu_Create.ToString(), policy => policy.AddRequirements(new PermissionAuthorizationRequirement(Operation.SystemMenu_Create)));
               // options.AddPolicy(Operation.SystemMenu_Delete.ToString(), policy => policy.AddRequirements(new PermissionAuthorizationRequirement(Operation.SystemMenu_Delete)));
                //options.AddPolicy(Operation.SystemMenu_Edit.ToString(), policy => policy.AddRequirements(new PermissionAuthorizationRequirement(Operation.SystemMenu_Edit)));
            });
        }
    }
}
