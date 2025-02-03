using EmpTracker.EmpService.Application.Exceptions;
using EmpTracker.EmpService.Application.Features.Employees.Commands;
using EmpTracker.EmpService.Core.Interfaces;
using EmpTracker.EventBus.Contracts;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmpTracker.EmpService.Application.Features.Employees.Handlers
{
    public class DeleteEmployeeCommandHandler(IPublishEndpoint publishEndpoint, IUnitOfWork unitOfWork, ILogger<DeleteEmployeeCommandHandler> logger) : IRequestHandler<DeleteEmployeeCommand>
    {
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<DeleteEmployeeCommandHandler> _logger = logger;

        public async Task Handle(DeleteEmployeeCommand command, CancellationToken cancellationToken)
        {
            var employee = await _unitOfWork.EmployeeManager.FirstOrDefaultAsync(x => x.EmployeeId == command.EmployeeId, cancellationToken) ?? throw new NotFoundException("Employee does not exist.");
            _unitOfWork.EmployeeManager.Remove(employee);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Employee deleted.");

            await _publishEndpoint.Publish(new EmployeeDeletionSuccess(employee.EmployeeId, employee.DepartmentId, employee.DesignationId), cancellationToken);
        }
    }
}
