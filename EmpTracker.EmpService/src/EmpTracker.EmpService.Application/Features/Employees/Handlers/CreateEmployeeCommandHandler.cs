﻿using EmpTracker.DptService.Core.Protos;
using EmpTracker.EmpService.Application.Dtos;
using EmpTracker.EmpService.Application.Exceptions;
using EmpTracker.EmpService.Application.Features.Employees.Commands;
using EmpTracker.EmpService.Core.Domain.Entities;
using EmpTracker.EmpService.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using static EmpTracker.DptService.Core.Protos.DepartmentGrpc;

namespace EmpTracker.EmpService.Application.Features.Employees.Handlers
{
    public class CreateEmployeeCommandHandler(DepartmentGrpcClient departmentGrpcClient, IMessageBus messageBus, IUnitOfWork unitOfWork, ILogger<CreateEmployeeCommandHandler> logger) : IRequestHandler<CreateEmployeeCommand>
    {
        private readonly IMessageBus _messageBus = messageBus;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly DepartmentGrpcClient _departmentGrpcClient = departmentGrpcClient;
        private readonly ILogger<CreateEmployeeCommandHandler> _logger = logger;

        public async Task Handle(CreateEmployeeCommand command, CancellationToken cancellationToken)
        {
            var reply = await _departmentGrpcClient.CheckIfDepartmentExistsAsync(new CheckIfDepartmentExistsRequest { DepartmentId = command.DepartmentId.ToString() }, cancellationToken: cancellationToken);
            if (!reply.Exists)
            {
                throw new NotFoundException("Department does not exist.");
            }
            
            var newEmployee = new Employee
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                PhoneNumber = command.PhoneNumber,
                DateOfBirth = command.DateOfBirth,
                DateOfJoining = command.DateOfJoining,
                Address = command.Address,
                City = command.City,
                State = command.State,
                Country = command.Country,
                PostalCode = command.PostalCode,
                IsActive = true,
                DepartmentId = command.DepartmentId,
                DesignationId = command.DesignationId,
            };

            await _unitOfWork.EmployeeManager.AddAsync(newEmployee, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Employee creaded.");
            await PublishMessage(newEmployee);
        }

        private async Task PublishMessage(Employee newEmployee)
        {
            var dto = new EmployeeMessageRequestDto
            {
                EmployeeId = newEmployee.EmployeeId,
                Email = newEmployee.Email,
                DepartmentId = newEmployee.DepartmentId,
                DesignationId = newEmployee.DesignationId
            };
            await _messageBus.PublishAsync(dto, "empTracker.direct", ExchangeType.Direct, "employee.created");
        }
    }
}
