using MediatR;

namespace EmpTracker.Identity.Application.Features.Users.Commands
{
    public class CreateUserForEmployeeCommand : IRequest
    {
        public Guid EmployeeId { get; set; }
        public required string Email { get; set; }
        public required string PasswordRaw { get; set; }
        public IEnumerable<string>? Roles { get; set; }
    }
}
