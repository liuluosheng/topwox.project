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

namespace WebService.Core
{
    public static class DependencyConfig
    {

        public static void Config(IServiceCollection services, IConfiguration configuration)
        {    
            services.AddScoped<IBaseRepository, BaseRepository>();
            services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
            services.AddSingleton(typeof(MongoClient), new MongoClient(configuration["AppSettings:Mongo"]));
            services.AddSingleton<IConfiguration>(configuration);
        }
    }
}
