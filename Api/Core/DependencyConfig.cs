using X.Core.IServices;
using X.Core.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.Data;
using X.Repository;


namespace Ew.Api.Core
{
    public static class DependencyConfig
    {

        public static void Config(IServiceCollection services)
        {
            services.AddScoped<DbContext, EwApiDBContext>();
            services.AddScoped<IBaseRepository, BaseRepository>();
            services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
        }
    }
}
