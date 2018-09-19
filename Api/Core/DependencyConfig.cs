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
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace Ew.Api.Core
{
    public static class DependencyConfig
    {

        public static void Config(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<DbContext, EwApiDBContext>();
            services.AddScoped<IBaseRepository, BaseRepository>();
            services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
            services.AddSingleton(typeof(MongoClient), new MongoClient(configuration["AppSettings:Mongo"]));
            services.AddSingleton<IConfiguration>(configuration);
        }
    }
}
