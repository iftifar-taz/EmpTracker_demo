using EmpTracker.Identity.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EmpTracker.Identity.Api.Configurations
{
    public static class DbConfiguration
    {
        public static async Task ApplyPendingMigrations(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DataContext>();
                db.Database.Migrate();
            }

            await DbSeeder.SeedUsersAndRolesAsync(app.ApplicationServices);
        }
    }
}
