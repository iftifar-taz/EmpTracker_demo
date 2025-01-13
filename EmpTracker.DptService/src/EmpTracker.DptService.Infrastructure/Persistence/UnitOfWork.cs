using EmpTracker.DptService.Core.Domain.Entities;
using EmpTracker.DptService.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmpTracker.DptService.Infrastructure.Persistence
{
    public class UnitOfWork(DataContext context) : IUnitOfWork, IDisposable
    {
        private readonly DataContext _context = context;
        private bool _disposed;

        public DbSet<Department> DepartmentManager { get; } = context.Departments;

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
