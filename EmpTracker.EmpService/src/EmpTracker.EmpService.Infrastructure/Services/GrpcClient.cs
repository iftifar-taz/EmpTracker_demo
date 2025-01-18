using EmpTracker.EmpService.Core.Interfaces;
using static EmpTracker.EmpService.Core.Protos.DepartmentGrpc;
using static EmpTracker.EmpService.Core.Protos.DesignationGrpc;

namespace EmpTracker.EmpService.Infrastructure.Services
{
    public class GrpcClient(DepartmentGrpcClient dptClient, DesignationGrpcClient dgicClient) : IGrpcClient
    {
        public DepartmentGrpcClient DepartmentService { get; init; } = dptClient;
        public DesignationGrpcClient DesignationService { get; init; } = dgicClient;
    }
}
