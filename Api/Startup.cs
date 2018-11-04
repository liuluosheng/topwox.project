using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Topwox.Data;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNet.OData.Routing.Conventions;
using Microsoft.AspNet.OData.Routing;
using CacheCow.Server.Core.Mvc;
using EasyCaching.InMemory;
using Autofac;
using MongoDB.Driver;
using Topwox.WebService.Core;
using Topwox.WebService.Core.Config;
using WebService.Core;

namespace Topwox.WebService.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(env.ContentRootPath)
            .AddEnvironmentVariables()
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                    .ConfigureApplicationPartManager(m => m.FeatureProviders.Add(new GenericTypeControllerFeatureProvider()))
                    .AddAuthorization()
                    .AddJsonFormatters();
            var assemblyName = Assembly.GetExecutingAssembly().FullName;
            services.AddDbContext<TopwoxApiDBContext>(options =>
            {
                options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"], b =>
                {
                    b.MigrationsAssembly(assemblyName);
                    b.UseRowNumberForPaging(); //兼容 server 2008 分页  
                });

            });
            // github.com/aliostad/CacheCow
            services.AddHttpCachingMvc();

            // github.com/dotnetcore/EasyCaching
            services.AddDefaultInMemoryCache();
            services.AddScoped<DbContext, TopwoxApiDBContext>();
            services.AddSingleton(new MongoClient(Configuration["AppSettings:Mongo"]));
            //  services.AddSingleton(DynamicOperation.CreateDynamicOperation());
            StartupServiceConfig.Config(services, Configuration);
            services.AddCors();
            services.AddOData();
            services.AddAutoMapper();
            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Configuration["AppSettings:IdentityServer"];
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "api";
                });
            return AutofacModule.Register(services, Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(police =>
            {
                police.AllowAnyOrigin();
                police.AllowAnyMethod();
                police.AllowAnyHeader();
            });
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
            app.UseMvc(b =>
            {
                b.Select().Expand().Filter().OrderBy().MaxTop(100).Count();
                IList<IODataRoutingConvention> conventions = ODataRoutingConventions.CreateDefault();
                conventions.Insert(0, new MatchRoutingConvention());
                b.MapODataServiceRoute("odata", "odata", ODataConfig.GetEdmModel(), new DefaultODataPathHandler(), conventions);

                //b.MapODataServiceRoute("odata", "odata", ODataConfig.GetEdmModel());

                b.EnableDependencyInjection();
                b.MapRoute(
                   name: "default",
                   template: "api/{controller}/{action}/{id?}");
            });
            AutoMapperConfig.MapperConfig();
        }
    }
}
