using EmpTracker.DgiService.Infrastructure.gRPC;

namespace EmpTracker.DgiService.Api.Configurations
{
    public static class GrpcConfiguration
    {
        public static IServiceCollection ConfigureGrpc(this IServiceCollection services)
        {
            services.AddGrpc();
            return services;
        }

        public static void MapGrpcServices(this IEndpointRouteBuilder app)
        {
            app.MapGrpcService<DesignationService>();
        }
    }
}
