using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IdentityServer4.AspNetIdentity;
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
using WebService.IdentityApi.Data;

namespace WebService.IdentityApi
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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
             .AddProfileService<ProfileService<SysUser>>()
             .AddAspNetIdentity<SysUser>();
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
            app.UseMvc();
        }
    }
}
