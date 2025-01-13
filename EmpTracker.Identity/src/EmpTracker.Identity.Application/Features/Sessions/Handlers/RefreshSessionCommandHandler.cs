using EmpTracker.Identity.Application.Dtos;
using EmpTracker.Identity.Application.Exceptions;
using EmpTracker.Identity.Application.Features.Sessions.Commands;
using EmpTracker.Identity.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmpTracker.Identity.Application.Features.Sessions.Handlers
{
    public class RefreshSessionCommandHandler(IUnitOfWork unitOfWork, IJwtTokenService jwtTokenService, ILogger<RefreshSessionCommandHandler> logger) : IRequestHandler<RefreshSessionCommand, SessionResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
        private readonly ILogger<RefreshSessionCommandHandler> _logger = logger;

        public async Task<SessionResponseDto> Handle(RefreshSessionCommand command, CancellationToken cancellationToken)
        {
            var principal = _jwtTokenService.GetPrincipalFromExpiredToken(command.Token);
            var user = await _unitOfWork.UserManager.FindByEmailAsync(command.Email);
            if (principal is null || user is null || user.RefreshToken != command.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new BadRequestException("Invalid credentials!");
            }

            var newJwtToken = await _jwtTokenService.GenerateToken(user);
            var newRefreshToken = _jwtTokenService.GenerateRefreshToken();
            var refrshTokenValidityIn = _jwtTokenService.GetRefrshTokenValidityIn();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(refrshTokenValidityIn);
            await _unitOfWork.UserManager.UpdateAsync(user);
            _logger.LogWarning($"{command.Email} created token with refresh token.");
            return new SessionResponseDto
            {
                Token = newJwtToken,
                RefreshToken = newRefreshToken,
            };
        }
    }
}
