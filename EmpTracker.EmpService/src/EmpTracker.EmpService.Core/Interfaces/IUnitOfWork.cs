using EmpTracker.EmpService.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmpTracker.EmpService.Core.Interfaces
{
    public interface IUnitOfWork
    {
        public DbSet<Employee> EmployeeManager { get; }

        Task<int> SaveChangesAsync();
    }
}
