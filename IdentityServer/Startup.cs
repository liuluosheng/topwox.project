using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Data;
using Ew.Core.Config;
using Ew.IdentityServer.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ew.IdentityServer
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
                    b.UseRowNumberForPaging();
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
             .AddAspNetIdentity<User>();


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
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
