using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Furni_Ecommerce_DashBoard.SeedData
{
    public static class IdentitySeedData
    {
        private const string ADMIN_ROLE = "Owner";
        private const string ADMIN_EMAIL = "Owner@iti.com";
        private const string ADMIN_PASSWORD = "Owner@123";
        private const string ADMIN_USERNAME = "Owner";
        private const string ADMIN_FIRSTNAME = "Mahmoud";
        private const string ADMIN_LASTNAME = "Ahmed";
        private const string ADMIN_PHONENUMBER = "01234567891";

        public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Create Admin role if it doesn't exist
            if (!await roleManager.RoleExistsAsync(ADMIN_ROLE))
            {
                await roleManager.CreateAsync(new IdentityRole(ADMIN_ROLE));
            }

            // Check if admin user exists
            var adminUser = await userManager.FindByEmailAsync(ADMIN_EMAIL);
            if (adminUser == null)
            {
                // Create the admin user with all properties
                adminUser = new ApplicationUser
                {
                    UserName = ADMIN_USERNAME,
                    Email = ADMIN_EMAIL,
                    PhoneNumber = ADMIN_PHONENUMBER,
                    EmailConfirmed = true,
                    FirstName = ADMIN_FIRSTNAME,
                    LastName = ADMIN_LASTNAME
                };

                var result = await userManager.CreateAsync(adminUser, ADMIN_PASSWORD);

                if (result.Succeeded)
                {
                    // Assign user to Admin role
                    await userManager.AddToRoleAsync(adminUser, ADMIN_ROLE);

                    // Only add claims if you need them in addition to properties
                    // await userManager.AddClaimAsync(adminUser, new Claim("FullName", $"{ADMIN_FIRSTNAME} {ADMIN_LASTNAME}"));
                }
                else
                {
                    throw new Exception("Failed to create default admin user: " +
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                // Ensure existing admin has the correct role
                if (!await userManager.IsInRoleAsync(adminUser, ADMIN_ROLE))
                {
                    await userManager.AddToRoleAsync(adminUser, ADMIN_ROLE);
                }
            }
        }
    }
}