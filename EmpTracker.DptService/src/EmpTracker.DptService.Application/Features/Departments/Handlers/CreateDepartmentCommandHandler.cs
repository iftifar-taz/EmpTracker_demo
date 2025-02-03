using EmpTracker.DptService.Application.Exceptions;
using EmpTracker.DptService.Application.Features.Departments.Commands;
using EmpTracker.DptService.Core.Domain.Entities;
using EmpTracker.DptService.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmpTracker.DptService.Application.Features.Departments.Handlers
{
    public class CreateDepartmentCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateDepartmentCommandHandler> logger) : IRequestHandler<CreateDepartmentCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<CreateDepartmentCommandHandler> _logger = logger;

        public async Task<Guid> Handle(CreateDepartmentCommand command, CancellationToken cancellationToken)
        {
            var department = await _unitOfWork.DepartmentManager.FirstOrDefaultAsync(x => x.DepartmentKey == command.DepartmentKey, cancellationToken);
            if (department != null)
            {
                throw new BadRequestException("Department already exists.");
            }
            var newDepartment = new Department
            {
                DepartmentName = command.DepartmentName,
                DepartmentKey = command.DepartmentKey,
                Description = command.Description,
            };

            await _unitOfWork.DepartmentManager.AddAsync(newDepartment, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Department creaded.");

            return newDepartment.DepartmentId;
        }
    }
}
