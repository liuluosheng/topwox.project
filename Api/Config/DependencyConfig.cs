using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ew.Api.Config
{
    public static class DependencyConfig
    {

        public static void Config(IServiceCollection services)
        {
            services.AddScoped<DbContext, EwApiDBContext>();
        }
    }
}
