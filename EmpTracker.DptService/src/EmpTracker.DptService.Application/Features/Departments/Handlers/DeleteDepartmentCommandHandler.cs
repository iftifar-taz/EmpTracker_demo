using EmpTracker.DptService.Application.Dtos;
using EmpTracker.DptService.Application.Exceptions;
using EmpTracker.DptService.Application.Features.Departments.Commands;
using EmpTracker.DptService.Core.Domain.Entities;
using EmpTracker.DptService.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace EmpTracker.DptService.Application.Features.Departments.Handlers
{
    public class DeleteDepartmentCommandHandler(IMessageBus messageBus, IUnitOfWork unitOfWork, ILogger<DeleteDepartmentCommandHandler> logger) : IRequestHandler<DeleteDepartmentCommand>
    {
        private readonly IMessageBus _messageBus = messageBus;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<DeleteDepartmentCommandHandler> _logger = logger;

        public async Task Handle(DeleteDepartmentCommand command, CancellationToken cancellationToken)
        {
            var department = await _unitOfWork.DepartmentManager.FirstOrDefaultAsync(x => x.DepartmentId == command.DepartmentId, cancellationToken) ?? throw new BadRequestException("Department does not exist.");
            _unitOfWork.DepartmentManager.Remove(department);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Department deleted.");

            await PublishMessage(department);
        }

        private async Task PublishMessage(Department department)
        {
            var dto = new DepartmentMessageRequestDto
            {
                DepartmentId = department.DepartmentId,
            };
            await _messageBus.PublishAsync(dto, "empTracker.direct", ExchangeType.Direct, "department.deleted");
        }
    }
}
