using EmpTracker.DptService.Application.Exceptions;
using EmpTracker.DptService.Application.Features.Departments.Commands;
using EmpTracker.DptService.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmpTracker.DptService.Application.Features.Departments.Handlers
{
    public class UpdateDepartmentEmployeeCountCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateDepartmentEmployeeCountCommandHandler> logger) : IRequestHandler<UpdateDepartmentEmployeeCountCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<UpdateDepartmentEmployeeCountCommandHandler> _logger = logger;

        public async Task Handle(UpdateDepartmentEmployeeCountCommand command, CancellationToken cancellationToken)
        {
            var department = await _unitOfWork.DepartmentManager.FirstOrDefaultAsync(x => x.DepartmentId == command.DepartmentId, cancellationToken) ?? throw new BadRequestException("Department does not exist.");
            department.EmployeeCount += command.Amount;

            _unitOfWork.DepartmentManager.Update(department);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Department updated.");
        }
    }
}
