using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Data;
using IdentityServer.Config;
using IdentityServer.Model;
using IdentityServer4.AspNetIdentity;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNet.OData.Routing.Conventions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebService.Core;
using ODataConfig = IdentityServer.Config.ODataConfig;

namespace IdentityServer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMvc();
            services.AddScoped<DbContext, EwIdentityDBContext>();
            var assemblyName = Assembly.GetExecutingAssembly().FullName;
            services.AddDbContext<EwIdentityDBContext>(options =>
            {
                options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"], b =>
                {
                    b.MigrationsAssembly(assemblyName);
                    b.UseRowNumberForPaging(); //server 2008  
                });
            });
            services.AddIdentity<User, Role>(options =>
            {
                options.Tokens.ChangePhoneNumberTokenProvider = "Phone";
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);

            })
              .AddEntityFrameworkStores<EwIdentityDBContext>()
              .AddDefaultTokenProviders();

            services.AddMvc();
            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
             .AddInMemoryIdentityResources(IdentityServiceConfig.GetIdentityResources())
             .AddInMemoryApiResources(IdentityServiceConfig.GetApiResources())
             .AddInMemoryClients(IdentityServiceConfig.GetClients())
             .AddAspNetIdentity<User>()
             .AddProfileService<CustomProfileService<User, Role>>();
            services.AddOData();
            builder.AddDeveloperSigningCredential(filename: "tempkey.rsa");

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(police => police.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseIdentityServer();
            app.UseStaticFiles();
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
        }
    }
}
