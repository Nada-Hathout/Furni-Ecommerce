using BusinessLogic.Repository;
using BusinessLogic.Service;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Furni_Ecommerce_Website
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<FurniDbContext>(options =>
            {
                options.UseLazyLoadingProxies().UseSqlServer(
                    builder.Configuration.GetConnectionString("cs"),
                    sql => sql.MigrationsAssembly("DataAccess")

                    );
               
            });
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
            });
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(op =>
            {
                op.Password.RequireNonAlphanumeric=false;
                op.Password.RequireUppercase=false;
            }).AddEntityFrameworkStores<FurniDbContext>();
            builder.Services.AddScoped<IUserService , UserService>();
            builder.Services.AddScoped<IUsersRepository, UserRepository>();
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IProductService,ProductService>();
            builder.Services.AddScoped<IProductRepository,ProductRepository>();

            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
