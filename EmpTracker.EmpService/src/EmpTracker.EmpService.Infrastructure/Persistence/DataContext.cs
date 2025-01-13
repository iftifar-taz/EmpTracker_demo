using EmpTracker.EmpService.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmpTracker.EmpService.Infrastructure.Persistence
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Employee>().HasIndex(u => u.LastName);
            builder.Entity<Employee>().HasIndex(u => u.Email).IsUnique();
            builder.Entity<Employee>().HasIndex(u => u.PhoneNumber).IsUnique();
        }
    }
}
