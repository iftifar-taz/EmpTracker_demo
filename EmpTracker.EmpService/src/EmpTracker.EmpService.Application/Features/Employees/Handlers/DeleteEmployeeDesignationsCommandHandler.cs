using EmpTracker.EmpService.Application.Features.Employees.Commands;
using EmpTracker.EmpService.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmpTracker.EmpService.Application.Features.Employees.Handlers
{
    public class DeleteEmployeeDesignationsCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteEmployeeDesignationsCommandHandler> logger) : IRequestHandler<DeleteEmployeeDesignationsCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<DeleteEmployeeDesignationsCommandHandler> _logger = logger;

        public async Task Handle(DeleteEmployeeDesignationsCommand command, CancellationToken cancellationToken)
        {
            var employees = await _unitOfWork.EmployeeManager.Where(x => x.DesignationId == command.DesignationId).ToListAsync(cancellationToken);

            foreach (var employee in employees)
            {
                employee.DesignationId = null;
            }
            _unitOfWork.EmployeeManager.UpdateRange(employees);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Employee updated.");
        }
    }
}
