namespace EmpTracker.EmpService.Api.Configurations
{
    public static class CorsConfiguration
    {
        public static void UseCorseForAll(this IApplicationBuilder app)
        {
            app.UseCors(options =>
            {
                options.AllowAnyHeader();
                options.AllowAnyMethod();
                options.AllowAnyOrigin();
            });
        }
    }
}
