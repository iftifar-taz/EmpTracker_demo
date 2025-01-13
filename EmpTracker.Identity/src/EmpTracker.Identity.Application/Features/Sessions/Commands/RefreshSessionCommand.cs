using EmpTracker.Identity.Application.Dtos;
using MediatR;

namespace EmpTracker.Identity.Application.Features.Sessions.Commands
{
    public class RefreshSessionCommand : IRequest<SessionResponseDto>
    {
        public required string Email { get; set; } = null!;
        public required string Token { get; set; }
        public required string RefreshToken { get; set; }
    }
}
