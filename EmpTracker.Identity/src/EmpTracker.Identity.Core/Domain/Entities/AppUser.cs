using Microsoft.AspNetCore.Identity;

namespace EmpTracker.Identity.Core.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public Guid? EmployeeId { get; set; }
        public bool ForceChangePassword { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
