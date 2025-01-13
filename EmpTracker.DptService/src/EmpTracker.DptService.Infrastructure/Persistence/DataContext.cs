using EmpTracker.DptService.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmpTracker.DptService.Infrastructure.Persistence
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Department>().HasIndex(u => u.DepartmentKey).IsUnique();
        }
    }
}
