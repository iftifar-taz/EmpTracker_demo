using EmpTracker.DgiService.Application.Exceptions;
using EmpTracker.DgiService.Application.Features.Designations.Commands;
using EmpTracker.DgiService.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmpTracker.DgiService.Application.Features.Designations.Handlers
{
    public class UpdateDesignationEmployeeCountCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateDesignationEmployeeCountCommandHandler> logger) : IRequestHandler<UpdateDesignationEmployeeCountCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<UpdateDesignationEmployeeCountCommandHandler> _logger = logger;

        public async Task Handle(UpdateDesignationEmployeeCountCommand command, CancellationToken cancellationToken)
        {
            var designation = await _unitOfWork.DesignationManager.FirstOrDefaultAsync(x => x.DesignationId == command.DesignationId, cancellationToken) ?? throw new BadRequestException("Designation does not exist.");
            designation.EmployeeCount += command.Amount;

            _unitOfWork.DesignationManager.Update(designation);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Designation updated.");
        }
    }
}
