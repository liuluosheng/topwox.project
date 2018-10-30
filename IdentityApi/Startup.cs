using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CacheCow.Server.Core.Mvc;
using EasyCaching.InMemory;
using IdentityServer4.AspNetIdentity;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNet.OData.Routing.Conventions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebService.Core;
using WebService.Core.Authorization;
using WebService.Identity.Api.Data;

namespace WebService.Identity.Api
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
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                 .ConfigureApplicationPartManager(m => m.FeatureProviders.Add(new GenericTypeControllerFeatureProvider()))
                 .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddScoped<DbContext, ApiIdEntityDBContext>();

            var assemblyName = Assembly.GetExecutingAssembly().FullName;

            services.AddDbContext<ApiIdEntityDBContext>(options =>
            {
                options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"], b =>
                {
                    b.MigrationsAssembly(assemblyName);
                    b.UseRowNumberForPaging(); //server 2008  
                });
            });

            // github.com/aliostad/CacheCow
            services.AddHttpCachingMvc();

            // github.com/dotnetcore/EasyCaching
            services.AddDefaultInMemoryCache();
            services.AddScoped<DbContext, ApiIdEntityDBContext>();
            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler<SysUser>>();
            DependencyConfig.Config(services, Configuration);
            services.AddCors();
            services.AddOData();
            services.AddIdentity<SysUser, SysRole>(options =>
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
              .AddEntityFrameworkStores<ApiIdEntityDBContext>()
              .AddDefaultTokenProviders();

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
             .AddAspNetIdentity<SysUser>()
             .AddProfileService<CustomProfileService<SysUser,SysRole>>();
            builder.AddDeveloperSigningCredential(filename: "tempkey.rsa");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // app.UseHsts();
            }
            //app.UseHttpsRedirection();   
            app.UseCors(police => police.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseIdentityServer();
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
        }
    }
}
