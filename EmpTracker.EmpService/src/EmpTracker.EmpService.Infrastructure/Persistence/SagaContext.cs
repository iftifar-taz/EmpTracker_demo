using EmpTracker.EmpService.Core.Domain.SagaState;
using Microsoft.EntityFrameworkCore;

namespace EmpTracker.EmpService.Infrastructure.Persistence
{
    public class SagaContext(DbContextOptions<SagaContext> options) : DbContext(options)
    {
        public DbSet<EmployeeCreationState> EmployeeCreationStates { get; set; }
        public DbSet<EmployeeDeletionState> EmployeeDeletionStates { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("saga");
        }
    }
}
