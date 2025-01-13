using EmpTracker.DgiService.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmpTracker.DgiService.Core.Interfaces
{
    public interface IUnitOfWork
    {
        public DbSet<Designation> DesignationManager { get; }

        Task<int> SaveChangesAsync();
    }
}
