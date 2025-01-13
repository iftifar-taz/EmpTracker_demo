using EmpTracker.DptService.Application.Dtos;
using MediatR;

namespace EmpTracker.DptService.Application.Features.Departments.Queries
{
    public class GetDepartmentQuery(Guid departmentId) : IRequest<DepartmentResponseDto>
    {
        public Guid DepartmentId { get; private set; } = departmentId;
    }
}
