using BusinessLogic.Repository;
using BusinessLogic.Service;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using System.IO;
using BusinessLogic.External_Service;

namespace Furni_Ecommerce_Website
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1) DbContext
            builder.Services.AddDbContext<FurniDbContext>(options =>
            {
                options.UseLazyLoadingProxies()
                       .UseSqlServer(builder.Configuration.GetConnectionString("cs"),
                                     sql => sql.MigrationsAssembly("DataAccess"));
            });

            // 2) Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<FurniDbContext>()
            .AddDefaultTokenProviders();

            // 3) Configure Application Cookie settings
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.SlidingExpiration = true;
                options.LoginPath = "/Auth/Login";
                options.AccessDeniedPath = "/Auth/Login";
            });

            // 4) Session & HttpContext
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });

            // 5) Data Protection for persistent session 
            builder.Services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "session-keys")))
                .SetApplicationName("FurniEcommerce")
                .SetDefaultKeyLifetime(TimeSpan.FromDays(90));

            // 6) Repositories & Services
            builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();
            builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
            builder.Services.AddScoped<IUsersRepository, UserRepository>();
            builder.Services.AddScoped<IAddressRepository, AddressRepository>();
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddScoped<GmailService>();
            builder.Services.AddScoped<IPurchaseConfirmationService,PurchaseConfirmationService>();
            builder.Services.AddScoped<CartItemService>();
            builder.Services.AddScoped<ProductService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICartItemService, CartItemService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddScoped<IAddressService, AddressService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IOrderItemService, OrderItemService>();
            builder.Services.AddScoped<IFavoriteService, FavoriteService>();
            builder.Services.AddScoped<ICartService, CartService>();

            // 7) MVC
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Create session-keys directory if not exists ---
            Directory.CreateDirectory(Path.Combine(app.Environment.ContentRootPath, "session-keys"));

            // Seed roles and admin user
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    await SeedRoles(roleManager);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding roles");
                }
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            // Important: UseSession must come before UseAuthentication and UseAuthorization
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );

            app.Run();
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }
        }
    }
}