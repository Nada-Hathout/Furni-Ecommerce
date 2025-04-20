using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.Service;
using BusinessLogic.Repository;

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

            //Add Repositories
            builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();

            //Add Services
            builder.Services.AddScoped<CartItemService>();
            builder.Services.AddScoped<ProductService>();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );

            app.Run();
        }
    }
}
