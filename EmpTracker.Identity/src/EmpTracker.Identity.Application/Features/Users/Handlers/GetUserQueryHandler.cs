using EmpTracker.Identity.Application.Dtos;
using EmpTracker.Identity.Application.Exceptions;
using EmpTracker.Identity.Application.Features.Users.Queries;
using EmpTracker.Identity.Core.Interfaces;
using MediatR;

namespace EmpTracker.Identity.Application.Features.Users.Handlers
{
    public class GetUserQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUserQuery, UserResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<UserResponseDto> Handle(GetUserQuery query, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserManager.FindByIdAsync(query.UserId) ?? throw new NotFoundException("User does not exist.");
            return new UserResponseDto
            {
                UserId = user.Id,
                Email = user.Email,
            };
        }
    }
}
