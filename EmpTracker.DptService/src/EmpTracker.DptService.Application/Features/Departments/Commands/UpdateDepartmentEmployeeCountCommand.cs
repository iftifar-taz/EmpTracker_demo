using MediatR;

namespace EmpTracker.DptService.Application.Features.Departments.Commands
{
    public class UpdateDepartmentEmployeeCountCommand(Guid departmentId, int amount) : IRequest
    {
        public Guid DepartmentId { get; private set; } = departmentId;
        public int Amount { get; private set; } = amount;
    }
}
