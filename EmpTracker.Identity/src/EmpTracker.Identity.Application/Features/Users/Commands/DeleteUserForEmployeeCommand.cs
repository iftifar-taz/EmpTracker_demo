using MediatR;

namespace EmpTracker.Identity.Application.Features.Users.Commands
{
    public class DeleteUserForEmployeeCommand(Guid employeeId) : IRequest
    {
        public Guid EmployeeId { get; private set; } = employeeId;
    }
}
