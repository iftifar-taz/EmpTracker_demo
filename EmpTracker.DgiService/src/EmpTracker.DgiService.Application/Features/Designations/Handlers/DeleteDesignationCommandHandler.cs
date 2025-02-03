using EmpTracker.DgiService.Application.Exceptions;
using EmpTracker.DgiService.Application.Features.Designations.Commands;
using EmpTracker.DgiService.Core.Interfaces;
using EmpTracker.EventBus.Contracts;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmpTracker.DgiService.Application.Features.Designations.Handlers
{
    public class DeleteDesignationCommandHandler(IPublishEndpoint publishEndpoint, IUnitOfWork unitOfWork, ILogger<DeleteDesignationCommandHandler> logger) : IRequestHandler<DeleteDesignationCommand>
    {
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<DeleteDesignationCommandHandler> _logger = logger;

        public async Task Handle(DeleteDesignationCommand command, CancellationToken cancellationToken)
        {
            var designation = await _unitOfWork.DesignationManager.FirstOrDefaultAsync(x => x.DesignationId == command.DesignationId, cancellationToken) ?? throw new BadRequestException("Designation does not exist.");
            _unitOfWork.DesignationManager.Remove(designation);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Designation deleted.");

            await _publishEndpoint.Publish(new DesignationDeletionSuccess(designation.DesignationId), cancellationToken);
        }
    }
}
