using EmpTracker.DptService.Application.Dtos;
using MediatR;

namespace EmpTracker.DptService.Application.Features.Departments.Queries
{
    public class GetDepartmentsQuery() : IRequest<IEnumerable<DepartmentResponseDto>>
    {
    }
}
