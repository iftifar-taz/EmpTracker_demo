using EmpTracker.DptService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EmpTracker.DptService.Api.Configurations
{
    public static class DbConfiguration
    {
        public static void ApplyPendingMigrations(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DataContext>();
            db.Database.Migrate();
        }
    }
}
