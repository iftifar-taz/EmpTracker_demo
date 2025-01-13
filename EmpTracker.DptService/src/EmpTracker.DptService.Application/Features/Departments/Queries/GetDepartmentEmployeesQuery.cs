using EmpTracker.DptService.Application.Dtos;
using MediatR;

namespace EmpTracker.DptService.Application.Features.Departments.Queries
{
    public class GetDepartmentEmployeesQuery(Guid departmentId) : IRequest<IEnumerable<DepartmentEmployeeResponseDto>>
    {
        public Guid DepartmentId { get; private set; } = departmentId;
    }
}
