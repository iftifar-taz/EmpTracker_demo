using EmpTracker.Identity.Application.Exceptions;
using EmpTracker.Identity.Application.Features.Users.Commands;
using EmpTracker.Identity.Core.Interfaces;
using MediatR;

namespace EmpTracker.Identity.Application.Features.Users.Handlers
{
    public class DeleteUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserManager.FindByIdAsync(command.UserId) ?? throw new NotFoundException("User does not exist.");
            await _unitOfWork.UserManager.DeleteAsync(user);
        }
    }
}
