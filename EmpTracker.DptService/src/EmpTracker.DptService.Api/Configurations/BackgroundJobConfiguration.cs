using Hangfire;
using Hangfire.SqlServer;

namespace EmpTracker.DptService.Api.Configurations
{
    public static class BackgroundJobConfiguration
    {
        public static IServiceCollection ConfigureBackgroundJob(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(x => x
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(configuration.GetConnectionString("Default"), new SqlServerStorageOptions
                {
                    SchemaName = "hangfire"
                }));

            services.AddHangfireServer();

            return services;
        }

        public static void UseBackgroundJobDashboard(this IEndpointRouteBuilder app)
        {
            app.MapHangfireDashboard();
        }
    }
}
