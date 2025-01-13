using EmpTracker.EmpService.Application.Dtos;
using MediatR;

namespace EmpTracker.EmpService.Application.Features.Employees.Queries
{
    public class GetEmployeeQuery(Guid employeeId) : IRequest<EmployeeResponseDto>
    {
        public Guid EmployeeId { get; private set; } = employeeId;
    }
}
