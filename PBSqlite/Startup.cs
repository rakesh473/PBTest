using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PBSqlite.Services;

namespace PBSqlite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            using var db = new ApplicationDbContext();
            db.Database.EnsureCreated();
            db.Database.Migrate();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages(options => { options.Conventions.AuthorizePage("/Admin"); });
            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login";
                    options.LogoutPath = "/Error";
                    options.AccessDeniedPath = "/Error";
                    options.Cookie.Name = "pb_session";
                });
            services.AddHttpContextAccessor();
            services.AddEntityFrameworkSqlite().AddDbContext<ApplicationDbContext>();
            services.AddTransient<ITableDataService, TableDataService>();
            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IRazorRenderService, RazorRenderService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
        }
    }
}