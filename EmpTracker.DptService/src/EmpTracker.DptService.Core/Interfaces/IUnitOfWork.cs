using EmpTracker.DptService.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmpTracker.DptService.Core.Interfaces
{
    public interface IUnitOfWork
    {
        public DbSet<Department> DepartmentManager { get; }

        Task<int> SaveChangesAsync();
    }
}
