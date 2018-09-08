using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using X.Data;
using Ew.Api.Config;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNet.OData.Routing.Conventions;
using Microsoft.AspNet.OData.Routing;
using Ew.Api.GenericController;

namespace Ew.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore(o => o.Conventions.Add(new GenericControllerRouteConvention()))
                    .ConfigureApplicationPartManager(m => m.FeatureProviders.Add(new GenericTypeControllerFeatureProvider()))
                    .AddAuthorization()
                    .AddJsonFormatters();
            var assemblyName = Assembly.GetExecutingAssembly().FullName;
            services.AddDbContext<EwApiDBContext>(options =>
            {
                options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"], b => b.MigrationsAssembly(assemblyName));
            });
            DependencyConfig.Config(services);
            services.AddCors();
            services.AddOData();
            services.AddAutoMapper();
            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "api";
                });
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
            AutoMapperConfig.InitAutoMapperConfig();
        }
    }
}
