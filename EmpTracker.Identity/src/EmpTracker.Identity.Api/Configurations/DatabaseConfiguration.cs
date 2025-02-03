using EmpTracker.Identity.Core.Domain.Entities;
using EmpTracker.Identity.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmpTracker.Identity.Api.Configurations
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

            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();

            return services;
        }

        public static async Task ApplyPendingMigrations(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DataContext>();
                db.Database.Migrate();
                var saga = scope.ServiceProvider.GetRequiredService<SagaContext>();
                saga.Database.Migrate();
            }

            await DbSeeder.SeedUsersAndRolesAsync(app.ApplicationServices);
        }
    }
}
