using EmpTracker.Identity.Core.Constants;
using EmpTracker.Identity.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace EmpTracker.Identity.Infrastructure.Persistence
{
    public static class DbSeeder
    {
        public static async Task SeedUsersAndRolesAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Seed roles
            var roles = new[] { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Seed admin user
            const string adminEmail = "admin@example.com";
            const string adminPassword = GlobalConstants.DefaultPassword;

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Seed user user
            const string userEmail = "user@example.com";
            const string userPassword = GlobalConstants.DefaultPassword;

            if (await userManager.FindByEmailAsync(userEmail) == null)
            {
                var userUser = new AppUser
                {
                    UserName = userEmail,
                    Email = userEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(userUser, userPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(userUser, "User");
                }
            }
        }
    }
}
