using MediatR;

namespace EmpTracker.Identity.Application.Features.Users.Commands
{
    public class DeleteUserCommand(string userId) : IRequest
    {
        public string UserId { get; private set; } = userId;
    }
}
