using EmpTracker.EmpService.Application.Dtos;
using MediatR;

namespace EmpTracker.EmpService.Application.Features.Employees.Queries
{
    public class GetEmployeesQuery() : IRequest<IEnumerable<EmployeeResponseDto>>
    {
    }
}
