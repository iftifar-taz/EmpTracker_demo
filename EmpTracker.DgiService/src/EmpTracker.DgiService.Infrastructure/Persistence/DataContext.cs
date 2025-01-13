using EmpTracker.DgiService.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmpTracker.DgiService.Infrastructure.Persistence
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public DbSet<Designation> Designations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Designation>().HasIndex(u => u.DesignationKey).IsUnique();
        }
    }
}
