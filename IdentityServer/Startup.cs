using IdentityServer.Data;
using IdentityServer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env) 
            => (_configuration, _env) = (configuration, env);

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AuthDbContext>(options 
                => options.UseNpgsql(_configuration.GetSection("DatabaseConnections")["AuthDbConnection"]));

            if (_env.IsDevelopment())
                services.AddIdentity<AppUser, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 4;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                })
                    .AddEntityFrameworkStores<AuthDbContext>()
                    .AddDefaultTokenProviders();
            else
            {
                services.AddIdentity<AppUser, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 7;
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                })
                    .AddEntityFrameworkStores<AuthDbContext>()
                    .AddDefaultTokenProviders();
            }

            services.AddControllersWithViews();

            services.AddIdentityServer(options => 
            {
                options.UserInteraction.LoginUrl = "/Auth/Login";
                options.UserInteraction.LogoutUrl = "Auth/Logout";
            })
                .AddAspNetIdentity<AppUser>()
                .AddDeveloperSigningCredential()
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryClients(Config.Clients)
                .AddInMemoryIdentityResources(Config.IdentityResources);

            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins("http://localhost:3000"); // react_client URL
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                });
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors("default");

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
