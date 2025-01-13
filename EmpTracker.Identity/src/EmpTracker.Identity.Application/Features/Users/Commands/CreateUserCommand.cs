using MediatR;

namespace EmpTracker.Identity.Application.Features.Users.Commands
{
    public class CreateUserCommand : IRequest
    {
        public required string Email { get; set; }
        public required string PasswordRaw { get; set; }
        public IEnumerable<string>? Roles { get; set; }
    }
}
