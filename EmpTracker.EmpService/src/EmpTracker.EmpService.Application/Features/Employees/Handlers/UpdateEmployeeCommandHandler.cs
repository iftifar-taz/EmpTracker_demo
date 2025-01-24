using EmpTracker.EmpService.Application.Exceptions;
using EmpTracker.EmpService.Application.Features.Employees.Commands;
using EmpTracker.EmpService.Core.Interfaces;
using EmpTracker.EmpService.Core.Protos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmpTracker.EmpService.Application.Features.Employees.Handlers
{
    public class UpdateEmployeeCommandHandler(IGrpcClient grpcClient, IUnitOfWork unitOfWork, ILogger<UpdateEmployeeCommandHandler> logger) : IRequestHandler<UpdateEmployeeCommand>
    {
        private readonly IGrpcClient _grpcClient = grpcClient;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<UpdateEmployeeCommandHandler> _logger = logger;

        public async Task Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken)
        {
            var employee = await _unitOfWork.EmployeeManager.FirstOrDefaultAsync(x => x.EmployeeId == command.EmployeeId, cancellationToken) ?? throw new BadRequestException("Employee does not exist.");

            var departmentCheck = await _grpcClient.DepartmentService.CheckIfDepartmentExistsAsync(new CheckIfDepartmentExistsRequest { DepartmentId = command.DepartmentId.ToString() }, cancellationToken: cancellationToken);
            if (!departmentCheck.Exists)
            {
                throw new NotFoundException("Department does not exist.");
            }

            var designationCheck = await _grpcClient.DesignationService.CheckIfDesignationExistsAsync(new CheckIfDesignationExistsRequest { DesignationId = command.DesignationId.ToString() }, cancellationToken: cancellationToken);
            if (!designationCheck.Exists)
            {
                throw new NotFoundException("Designation does not exist.");
            }

            employee.FirstName = command.FirstName;
            employee.LastName = command.LastName;
            employee.Email = command.Email;
            employee.PhoneNumber = command.PhoneNumber;
            employee.DateOfBirth = command.DateOfBirth;
            employee.DateOfJoining = command.DateOfJoining;
            employee.DateOfResignation = command.DateOfResignation;
            employee.Address = command.Address;
            employee.City = command.City;
            employee.State = command.State;
            employee.Country = command.Country;
            employee.PostalCode = command.PostalCode;
            employee.IsActive = true;
            employee.DepartmentId = command.DepartmentId;
            employee.DesignationId = command.DesignationId;

            _unitOfWork.EmployeeManager.Update(employee);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Employee updated.");
        }
    }
}
