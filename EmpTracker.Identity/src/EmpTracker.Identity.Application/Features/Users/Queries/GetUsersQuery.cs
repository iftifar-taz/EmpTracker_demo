using EmpTracker.Identity.Application.Dtos;
using MediatR;

namespace EmpTracker.Identity.Application.Features.Users.Queries
{
    public class GetUsersQuery : IRequest<IEnumerable<UserResponseDto>>
    {
    }
}
