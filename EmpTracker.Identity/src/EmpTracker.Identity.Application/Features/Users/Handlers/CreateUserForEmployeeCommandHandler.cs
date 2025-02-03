using EmpTracker.Identity.Application.Exceptions;
using EmpTracker.Identity.Application.Features.Users.Commands;
using EmpTracker.Identity.Core.Domain.Entities;
using EmpTracker.Identity.Core.Interfaces;
using MediatR;

namespace EmpTracker.Identity.Application.Features.Users.Handlers
{
    public class CreateUserForEmployeeCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateUserForEmployeeCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<string> Handle(CreateUserForEmployeeCommand command, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserManager.FindByEmailAsync(command.Email);
            if (user != null)
            {
                throw new ConflictException("User already exist.");
            }

            var nweUser = new AppUser
            {
                EmployeeId = command.EmployeeId,
                ForceChangePassword = true,
                Email = command.Email,
                UserName = command.Email
            };

            var result = await _unitOfWork.UserManager.CreateAsync(nweUser, command.PasswordRaw);

            if (!result.Succeeded)
            {
                throw new UserCreationException(result.Errors.FirstOrDefault()?.Description ?? string.Empty);
            }

            if (command.Roles is null)
            {
                await _unitOfWork.UserManager.AddToRoleAsync(nweUser, "User");
            }
            else
            {
                await _unitOfWork.UserManager.AddToRolesAsync(nweUser, command.Roles);
            }

            return nweUser.Id;
        }
    }
}
