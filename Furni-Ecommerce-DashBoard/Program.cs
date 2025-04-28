using BusinessLogic.Repository;
using BusinessLogic.Service;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.Repository;
using BusinessLogic.Service;
using Furni_Ecommerce_DashBoard.SeedData;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Furni_Ecommerce_DashBoard
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Database Context
            builder.Services.AddDbContext<FurniDbContext>(options =>
            {
                options.UseLazyLoadingProxies().UseSqlServer(
                    builder.Configuration.GetConnectionString("cs"),
                    sql => sql.MigrationsAssembly("DataAccess")
                );
            });

            // Add Category Services (without authentication for now)
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();

                    );
            });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(op =>
            {
                op.User.RequireUniqueEmail = true;
                op.Password.RequireNonAlphanumeric = false;
                op.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<FurniDbContext>().AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Auth/Login";
                options.AccessDeniedPath = "/Auth/Login";
            });
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUsersRepository,UserRepository>();

            // Session & HttpContext
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");

                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.UseStaticFiles(); // Add this before MapStaticAssets()
            app.UseRouting();

            app.UseAuthentication(); // This must come before Authorization
            app.UseAuthorization();

            app.UseSession(); // Add session middleware

            // Seed admin user
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await IdentitySeedData.SeedAdminAsync(services);
            }

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Auth}/{action=Login}/{id?}")
                .WithStaticAssets();

            await app.RunAsync();
        }
    }
}