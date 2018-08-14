using Core.IServices;
using Core.Service;
using Data;
using Data.Entitys;
using Data.Repository;
using Data.Repository.Interface;
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
            services.AddScoped<IBaseRepository, BaseRepository>();
            services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
        }
    }
}
