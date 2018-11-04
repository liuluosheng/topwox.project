using Topwox.Core.IServices;
using Topwox.Core.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Topwox.Data;
using Topwox.Core.Repository;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace Topwox.WebService.Core
{
    public static class StartupServiceConfig
    {

        public static void Config(IServiceCollection services, IConfiguration configuration)
        {    
         
        }
    }
}
