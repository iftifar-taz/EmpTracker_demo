using EmpTracker.DgiService.Application.Dtos;
using EmpTracker.DgiService.Application.Exceptions;
using EmpTracker.DgiService.Application.Features.Designations.Commands;
using EmpTracker.DgiService.Core.Domain.Entities;
using EmpTracker.DgiService.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace EmpTracker.DgiService.Application.Features.Designations.Handlers
{
    public class DeleteDesignationCommandHandler(IMessageBus messageBus, IUnitOfWork unitOfWork, ILogger<DeleteDesignationCommandHandler> logger) : IRequestHandler<DeleteDesignationCommand>
    {
        private readonly IMessageBus _messageBus = messageBus;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<DeleteDesignationCommandHandler> _logger = logger;

        public async Task Handle(DeleteDesignationCommand command, CancellationToken cancellationToken)
        {
            var designation = await _unitOfWork.DesignationManager.FirstOrDefaultAsync(x => x.DesignationId == command.DesignationId, cancellationToken) ?? throw new BadRequestException("Designation does not exist.");
            _unitOfWork.DesignationManager.Remove(designation);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Designation deleted.");

            await PublishMessage(designation);
        }

        private async Task PublishMessage(Designation designation)
        {
            var dto = new DesignationMessageRequestDto
            {
                DesignationId = designation.DesignationId,
            };
            await _messageBus.PublishAsync(dto, "empTracker.direct", ExchangeType.Direct, "designation.deleted");
        }
    }
}
