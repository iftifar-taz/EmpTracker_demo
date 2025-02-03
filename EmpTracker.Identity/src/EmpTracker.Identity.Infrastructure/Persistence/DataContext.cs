using EmpTracker.Identity.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmpTracker.Identity.Infrastructure.Persistence
{
    public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<AppUser>(options)
    {
        public DbSet<Permission> Permissions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("app");

            builder.Entity<Permission>().HasIndex(u => u.PermissionKey).IsUnique();
            builder.Entity<Permission>().HasIndex(u => u.ServiceName);
        }
    }
}
