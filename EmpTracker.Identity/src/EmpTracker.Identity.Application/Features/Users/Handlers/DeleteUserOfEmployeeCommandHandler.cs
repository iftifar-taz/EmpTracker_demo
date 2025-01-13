using EmpTracker.Identity.Application.Exceptions;
using EmpTracker.Identity.Application.Features.Users.Commands;
using EmpTracker.Identity.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmpTracker.Identity.Application.Features.Users.Handlers
{
    public class DeleteUserOfEmployeeCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserOfEmployeeCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(DeleteUserOfEmployeeCommand command, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserManager.Users.FirstOrDefaultAsync(x => x.EmployeeId == command.EmployeeId, cancellationToken) ?? throw new NotFoundException("User does not exist.");
            await _unitOfWork.UserManager.DeleteAsync(user);
        }
    }
}
