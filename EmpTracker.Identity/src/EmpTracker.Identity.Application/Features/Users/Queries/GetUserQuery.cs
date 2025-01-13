using EmpTracker.Identity.Application.Dtos;
using MediatR;

namespace EmpTracker.Identity.Application.Features.Users.Queries
{
    public class GetUserQuery(string userId) : IRequest<UserResponseDto>
    {
        public string UserId { get; private set; } = userId;
    }
}
