using MediatR;

namespace EmpTracker.EmpService.Application.Features.Employees.Commands
{
    public class DeleteEmployeeDepartmentsCommand(Guid departmentId) : IRequest
    {
        public Guid DepartmentId { get; private set; } = departmentId;
    }
}
