using EmpTracker.EmpService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EmpTracker.EmpService.Api.Configurations
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
