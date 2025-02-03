using EmpTracker.DptService.Application.Exceptions;
using EmpTracker.DptService.Application.Features.Departments.Commands;
using EmpTracker.DptService.Core.Interfaces;
using EmpTracker.EventBus.Contracts;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmpTracker.DptService.Application.Features.Departments.Handlers
{
    public class DeleteDepartmentCommandHandler(IPublishEndpoint publishEndpoint, IUnitOfWork unitOfWork, ILogger<DeleteDepartmentCommandHandler> logger) : IRequestHandler<DeleteDepartmentCommand>
    {
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<DeleteDepartmentCommandHandler> _logger = logger;

        public async Task Handle(DeleteDepartmentCommand command, CancellationToken cancellationToken)
        {
            var department = await _unitOfWork.DepartmentManager.FirstOrDefaultAsync(x => x.DepartmentId == command.DepartmentId, cancellationToken) ?? throw new BadRequestException("Department does not exist.");
            _unitOfWork.DepartmentManager.Remove(department);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Department deleted.");

            await _publishEndpoint.Publish(new DepartmentDeletionSuccess(department.DepartmentId), cancellationToken);
        }
    }
}
