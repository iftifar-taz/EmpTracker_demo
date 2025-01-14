using Microsoft.AspNetCore.RateLimiting;

namespace EmpTracker.ApiGateway.Configurations
{
    public static class ApplicationConfiguration
    {
        public static IServiceCollection ConfigureRateLImiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.AddSlidingWindowLimiter("slidingWindowLimiterPolicy", opt =>
                {
                    opt.PermitLimit = 4;
                    opt.Window = TimeSpan.FromSeconds(30);
                    opt.SegmentsPerWindow = 1;
                    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    opt.QueueLimit = 1;
                });
                options.OnRejected = (context, _) =>
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.HttpContext.Response.ContentType = "application/json";
                    context.HttpContext.Response.WriteAsJsonAsync(new { message = "Too many requests. Please try again later." });
                    return new ValueTask();
                };
            });
            return services;
        }
    }
}
