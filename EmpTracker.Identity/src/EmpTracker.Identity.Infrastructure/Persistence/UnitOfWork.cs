using EmpTracker.Identity.Core.Domain.Entities;
using EmpTracker.Identity.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmpTracker.Identity.Infrastructure.Persistence
{
    public class UnitOfWork(DataContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager) : IUnitOfWork
    {
        private readonly DataContext _context = context;
        private bool _disposed;

        public UserManager<AppUser> UserManager { get; } = userManager;
        public RoleManager<IdentityRole> RoleManager { get; } = roleManager;
        public DbSet<Permission> PermissionManager { get; } = context.Permissions;

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
