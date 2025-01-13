using EmpTracker.EmpService.Application.Dtos;
using EmpTracker.EmpService.Application.Exceptions;
using EmpTracker.EmpService.Application.Features.Employees.Commands;
using EmpTracker.EmpService.Core.Domain.Entities;
using EmpTracker.EmpService.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace EmpTracker.EmpService.Application.Features.Employees.Handlers
{
    public class DeleteEmployeeCommandHandler(IMessageBus messageBus, IUnitOfWork unitOfWork, ILogger<DeleteEmployeeCommandHandler> logger) : IRequestHandler<DeleteEmployeeCommand>
    {
        private readonly IMessageBus _messageBus = messageBus;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<DeleteEmployeeCommandHandler> _logger = logger;

        public async Task Handle(DeleteEmployeeCommand command, CancellationToken cancellationToken)
        {
            var employee = await _unitOfWork.EmployeeManager.FirstOrDefaultAsync(x => x.EmployeeId == command.EmployeeId, cancellationToken) ?? throw new BadRequestException("Employee does not exist.");
            _unitOfWork.EmployeeManager.Remove(employee);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Employee deleted.");

            await PublishMessage(employee);
        }

        private async Task PublishMessage(Employee newEmployee)
        {
            var dto = new EmployeeMessageRequestDto
            {
                EmployeeId = newEmployee.EmployeeId,
                DepartmentId = newEmployee.DepartmentId,
                DesignationId = newEmployee.DesignationId
            };
            await _messageBus.PublishAsync(dto, "empTracker.direct", ExchangeType.Direct, "employee.deleted");
        }
    }
}
