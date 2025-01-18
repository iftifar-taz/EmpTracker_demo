using EmpTracker.EmpService.Core.Interfaces;
using EmpTracker.EmpService.Core.Protos;
using EmpTracker.EmpService.Infrastructure.Services;

namespace EmpTracker.EmpService.Api.Configurations
{
    public static class GrpcConfiguration
    {
        public static IServiceCollection ConfigureGrpc(this IServiceCollection services, IConfiguration configuration)
        {
            var gRPCSettings = configuration.GetSection("gRPC");

            services.AddGrpcClient<DepartmentGrpc.DepartmentGrpcClient>(o =>
            {
                o.Address = new Uri(gRPCSettings.GetSection("DepartmentService").Value!);
            });
            services.AddGrpcClient<DesignationGrpc.DesignationGrpcClient>(o =>
            {
                o.Address = new Uri(gRPCSettings.GetSection("DesignationService").Value!);
            });

            services.AddSingleton<IGrpcClient, GrpcClient>();
            return services;
        }
    }
}
