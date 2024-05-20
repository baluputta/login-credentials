using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using MVCDHProject.Models;

namespace MVCDHProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews(configure =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                configure.Filters.Add(new AuthorizeFilter(policy));
            });
            builder.Services.AddScoped<ICustomerDAL, CustomerSqlDAL>();
            builder.Services.AddDbContext<MVCCoreDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConStr")));
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<MVCCoreDbContext>().AddDefaultTokenProviders();

            builder.Services.AddAuthentication()
                .AddGoogle(options =>
                {
                 options.ClientId = "209665950237";
                 options.ClientSecret = "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";
                })
                .AddFacebook(options =>
                {
                    options.AppId = "1292461108030407";
                    options.AppSecret = "fb618bc0b82a8bbac65b6126e1795305";
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}