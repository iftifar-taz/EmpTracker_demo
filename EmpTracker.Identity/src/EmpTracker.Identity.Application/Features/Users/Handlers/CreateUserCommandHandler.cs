using EmpTracker.Identity.Application.Exceptions;
using EmpTracker.Identity.Application.Features.Users.Commands;
using EmpTracker.Identity.Core.Domain.Entities;
using EmpTracker.Identity.Core.Interfaces;
using MediatR;

namespace EmpTracker.Identity.Application.Features.Users.Handlers
{
    public class CreateUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateUserCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<string> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserManager.FindByEmailAsync(command.Email);
            if (user != null)
            {
                throw new ConflictException("User already exist.");
            }

            var newUser = new AppUser
            {
                Email = command.Email,
                UserName = command.Email
            };

            var result = await _unitOfWork.UserManager.CreateAsync(newUser, command.PasswordRaw);

            if (!result.Succeeded)
            {
                throw new UserCreationException(result.Errors.FirstOrDefault()?.Description ?? string.Empty);
            }

            if (command.Roles is null)
            {
                await _unitOfWork.UserManager.AddToRoleAsync(newUser, "User");
            }
            else
            {
                await _unitOfWork.UserManager.AddToRolesAsync(newUser, command.Roles);
            }

            return newUser.Id;
        }
    }
}
