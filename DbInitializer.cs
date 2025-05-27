using Hotel_Management.Models;
using Microsoft.AspNetCore.Identity;

namespace Hotel_Management
{
    public class DbInitializer
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = { "Admin", "Customer", "Staff" };

            // Create roles if they do not exist
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var identityRole = new IdentityRole(role);
                    await roleManager.CreateAsync(identityRole);
                }
            }

            // Create an admin user if it does not exist
            string adminUsername = "admin";
            string adminEmail = "admin@gmail.com";
            string adminPassword = "Admin@123";

            var adminUser = await userManager.FindByNameAsync(adminUsername);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminUsername,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var createAdminResult = await userManager.CreateAsync(adminUser, adminPassword);
                if (createAdminResult.Succeeded)
                {
                    // Assign the Admin role to the admin user
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
