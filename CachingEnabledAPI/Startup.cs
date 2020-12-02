using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CachingEnabledAPI.Configurations;
using CachingEnabledAPI.Models;
using CachingEnabledAPI.Data;
using CachingEnabledAPI.Repositories;
using CachingEnabledAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CachingEnabledAPI.Services.Interfaces;
using CachingEnabledAPI.Repositories.Interfaces;
using CachingEnabledAPI.Repositories.Implemetations;
using CachingEnabledAPI.Services.Implementations;

namespace CachingEnabledAPI
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
            services.AddControllersWithViews();

            #region Cache Config
            services.Configure<CacheConfiguration>(Configuration.GetSection("CacheConfiguration"));

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379";
                // options.InstanceName = "Inventory";
            });

            //For In-Memory Caching
            services.AddMemoryCache();
            services.AddTransient<MemoryCacheService>();
            services.AddTransient<RedisCacheService>();
            services.AddTransient<Func<CacheTech, ICacheService>>(serviceProvider => key =>
            {
                switch (key)
                {
                    case CacheTech.Memory:
                        return serviceProvider.GetService<MemoryCacheService>();
                    case CacheTech.Redis:
                        return serviceProvider.GetService<RedisCacheService>();
                    default:
                        return serviceProvider.GetService<MemoryCacheService>();
                }
            });
            #endregion

            #region HangFire
            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("SqlServerConnection")));
            services.AddHangfireServer();
            #endregion

            #region DB Connection

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                        Configuration.GetConnectionString("SqlServerConnection"),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
                
            });


            //services.AddDbContext<ApplicationDbContext>(options =>
            //       options.UseMySql(
            //           Configuration.GetConnectionString("MySqlConnection"),
            //           b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            #endregion

            #region Repositories
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            #endregion

            #region Services
            services.AddTransient<ICustomerService, CustomerService>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseHangfireDashboard("/jobs");
        }
    }
}
