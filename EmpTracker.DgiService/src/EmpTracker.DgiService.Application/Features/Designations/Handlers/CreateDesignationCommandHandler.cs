using EmpTracker.DgiService.Application.Exceptions;
using EmpTracker.DgiService.Application.Features.Designations.Commands;
using EmpTracker.DgiService.Core.Domain.Entities;
using EmpTracker.DgiService.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmpTracker.DgiService.Application.Features.Designations.Handlers
{
    public class CreateDesignationCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateDesignationCommandHandler> logger) : IRequestHandler<CreateDesignationCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<CreateDesignationCommandHandler> _logger = logger;

        public async Task Handle(CreateDesignationCommand command, CancellationToken cancellationToken)
        {
            var Designation = await _unitOfWork.DesignationManager.FirstOrDefaultAsync(x => x.DesignationKey == command.DesignationKey, cancellationToken);
            if (Designation != null)
            {
                throw new BadRequestException("Designation already exists.");
            }
            var newDesignation = new Designation
            {
                DesignationName = command.DesignationName,
                DesignationKey = command.DesignationKey,
                Description = command.Description,
            };

            await _unitOfWork.DesignationManager.AddAsync(newDesignation, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Designation creaded.");
        }
    }
}
