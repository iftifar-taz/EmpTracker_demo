using EmpTracker.DptService.Core.Domain.SagaState;
using Microsoft.EntityFrameworkCore;

namespace EmpTracker.DptService.Infrastructure.Persistence
{
    public class SagaContext(DbContextOptions<SagaContext> options) : DbContext(options)
    {
        public DbSet<DepartmentDeletionState> DepartmentDeletionStates { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("saga");
        }
    }
}
