using EmpTracker.EmpService.Core.Domain.Entities;
using EmpTracker.EmpService.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmpTracker.EmpService.Infrastructure.Persistence
{
    public class UnitOfWork(DataContext context) : IUnitOfWork, IDisposable
    {
        private readonly DataContext _context = context;
        private bool _disposed;

        public DbSet<Employee> EmployeeManager { get; } = context.Employees;

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
