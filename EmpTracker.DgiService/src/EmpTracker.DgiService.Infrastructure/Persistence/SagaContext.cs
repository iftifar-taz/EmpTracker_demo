using EmpTracker.DgiService.Core.Domain.SagaState;
using Microsoft.EntityFrameworkCore;

namespace EmpTracker.DgiService.Infrastructure.Persistence
{
    public class SagaContext(DbContextOptions<SagaContext> options) : DbContext(options)
    {
        public DbSet<DesignationDeletionState> DesignationDeletionStates { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("saga");
        }
    }
}
