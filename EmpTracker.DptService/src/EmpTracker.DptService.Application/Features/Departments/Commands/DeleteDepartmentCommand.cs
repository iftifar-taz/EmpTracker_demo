using MediatR;

namespace EmpTracker.DptService.Application.Features.Departments.Commands
{
    public class DeleteDepartmentCommand(Guid departmentId) : IRequest
    {
        public Guid DepartmentId { get; private set; } = departmentId;
    }
}
