using EmpTracker.Identity.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmpTracker.Identity.Core.Interfaces
{
    public interface IUnitOfWork
    {
        public UserManager<AppUser> UserManager { get; }
        public RoleManager<IdentityRole> RoleManager { get; }
        public DbSet<Permission> PermissionManager { get; }

        Task<int> SaveChangesAsync();
    }
}
