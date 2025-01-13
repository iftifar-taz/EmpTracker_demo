using System.Security.Claims;
using EmpTracker.Identity.Core.Domain.Entities;

namespace EmpTracker.Identity.Core.Interfaces
{
    public interface IJwtTokenService
    {
        Task<string> GenerateToken(AppUser user);
        string GenerateRefreshToken();
        int GetRefrshTokenValidityIn();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
