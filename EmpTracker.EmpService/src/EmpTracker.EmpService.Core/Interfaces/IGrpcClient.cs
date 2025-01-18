using static EmpTracker.EmpService.Core.Protos.DepartmentGrpc;
using static EmpTracker.EmpService.Core.Protos.DesignationGrpc;

namespace EmpTracker.EmpService.Core.Interfaces
{
    public interface IGrpcClient
    {
        public DepartmentGrpcClient DepartmentService { get; init; }
        public DesignationGrpcClient DesignationService { get; init; }
    }
}
