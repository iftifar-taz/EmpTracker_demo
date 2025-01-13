using EmpTracker.EmpService.Application.Features.Employees.Commands;
using EmpTracker.EmpService.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmpTracker.EmpService.Application.Features.Employees.Handlers
{
    public class DeleteEmployeeDepartmentsCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteEmployeeDepartmentsCommandHandler> logger) : IRequestHandler<DeleteEmployeeDepartmentsCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<DeleteEmployeeDepartmentsCommandHandler> _logger = logger;

        public async Task Handle(DeleteEmployeeDepartmentsCommand command, CancellationToken cancellationToken)
        {
            var employees = await _unitOfWork.EmployeeManager.Where(x => x.DepartmentId == command.DepartmentId).ToListAsync(cancellationToken);

            foreach (var employee in employees)
            {
                employee.DepartmentId = null;
            }
            _unitOfWork.EmployeeManager.UpdateRange(employees);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Employee updated.");
        }
    }
}
