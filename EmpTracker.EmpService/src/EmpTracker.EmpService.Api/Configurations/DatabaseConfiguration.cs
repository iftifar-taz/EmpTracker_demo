using EmpTracker.EmpService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EmpTracker.EmpService.Api.Configurations
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(builder =>
               builder.UseSqlServer(configuration.GetConnectionString("Default"), m =>
               {
                   m.MigrationsAssembly(typeof(DataContext).Assembly.GetName().Name);
                   m.MigrationsHistoryTable($"__{nameof(DataContext)}_EFMigrationsHistory", "app");
               }));

            return services;
        }

        public static void ApplyPendingMigrations(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DataContext>();
            db.Database.Migrate();
            var saga = scope.ServiceProvider.GetRequiredService<SagaContext>();
            saga.Database.Migrate();
        }
    }
}
