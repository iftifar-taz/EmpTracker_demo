using Serilog;

namespace EmpTracker.EmpService.Api.Configurations
{
    public static class LogingConfiguration
    {
        public static WebApplicationBuilder ConfigureLoging(this WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            builder.Host.UseSerilog();
            return builder;
        }

        public static void UseRequestLogging(this IApplicationBuilder app)
        {
            app.UseSerilogRequestLogging();
        }
    }
}
