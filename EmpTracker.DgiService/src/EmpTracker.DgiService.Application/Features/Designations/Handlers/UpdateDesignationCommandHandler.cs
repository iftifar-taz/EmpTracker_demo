using EmpTracker.DgiService.Application.Exceptions;
using EmpTracker.DgiService.Application.Features.Designations.Commands;
using EmpTracker.DgiService.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmpTracker.DgiService.Application.Features.Designations.Handlers
{
    public class UpdateDesignationCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateDesignationCommandHandler> logger) : IRequestHandler<UpdateDesignationCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<UpdateDesignationCommandHandler> _logger = logger;

        public async Task Handle(UpdateDesignationCommand command, CancellationToken cancellationToken)
        {
            var designation = await _unitOfWork.DesignationManager.FirstOrDefaultAsync(x => x.DesignationId == command.DesignationId, cancellationToken) ?? throw new BadRequestException("Designation does not exist.");
            designation.DesignationName = command.DesignationName;
            designation.DesignationKey = command.DesignationKey;
            designation.Description = command.Description;

            _unitOfWork.DesignationManager.Update(designation);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Designation updated.");
        }
    }
}
