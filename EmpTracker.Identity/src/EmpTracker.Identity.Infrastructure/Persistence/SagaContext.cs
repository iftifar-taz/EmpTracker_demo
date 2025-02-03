using EmpTracker.Identity.Core.Domain.SagaState;
using Microsoft.EntityFrameworkCore;

namespace EmpTracker.Identity.Infrastructure.Persistence
{
    public class SagaContext(DbContextOptions<SagaContext> options) : DbContext(options)
    {
        public DbSet<PermissionSyncState> PermissionSyncStates { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("saga");
        }
    }
}
