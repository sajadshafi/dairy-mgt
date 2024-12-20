using DairyManagementSystem.Models;
using DairyManagementSystem.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using NToastNotify;

namespace DairyManagementSystem {
   public class Program {
      public static void Main(string[] args) {
         var builder = WebApplication.CreateBuilder(args);
         // Add services to the container.
         builder.Services.AddControllersWithViews().AddNToastNotifyToastr(new ToastrOptions {
            ProgressBar = true,
            TimeOut = 3000
         });

         IConfiguration _config = builder.Configuration;
         builder.Services.AddDbContext<ApplicationDbContext>(options => {
            options.UseSqlServer(_config.GetConnectionString("DefaultConnection"));
         });
         builder.Services.AddIdentity<SystemUser, SystemRole>(options =>
         {
             options.Password.RequireNonAlphanumeric = false;
             options.Password.RequireUppercase= false;
             options.Password.RequireLowercase= false;
             options.Password.RequireDigit= false;
         })
             .AddEntityFrameworkStores<ApplicationDbContext>();
         builder.Services.AddServices();
         builder.Services.Configure<IdentityOptions>(options => {
            options.Password.RequireUppercase = false;
         });

         builder.Services.ConfigureApplicationCookie(options =>
         {
            //Location for your Custom Access Denied Page
            options.AccessDeniedPath = "/auth/accessDenied";

            //Location for your Custom Login Page
            options.LoginPath = "/auth/login";
         });

         var app = builder.Build();
         // Configure the HTTP request pipeline.
         if(!app.Environment.IsDevelopment()) {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
         }
         app.UseHttpsRedirection();
         app.UseStaticFiles();
         app.UseRouting();
         app.UseAuthentication();
         app.UseAuthorization();
         app.UseNToastNotify();
         app.MapControllerRoute(
             name: "default",
             pattern: "{controller=Dashboard}/{action=Index}/{id?}");
         app.Run();
      }
   }
}