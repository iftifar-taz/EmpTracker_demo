using MediatR;

namespace EmpTracker.EmpService.Application.Features.Employees.Commands
{
    public class DeleteEmployeeCommand(Guid employeeId) : IRequest
    {
        public Guid EmployeeId { get; private set; } = employeeId;
    }
}
