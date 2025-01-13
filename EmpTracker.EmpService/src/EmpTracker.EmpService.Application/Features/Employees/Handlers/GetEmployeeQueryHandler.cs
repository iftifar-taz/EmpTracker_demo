using EmpTracker.EmpService.Application.Dtos;
using EmpTracker.EmpService.Application.Exceptions;
using EmpTracker.EmpService.Application.Features.Employees.Queries;
using EmpTracker.EmpService.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmpTracker.EmpService.Application.Features.Employees.Handlers
{
    public class GetEmployeeQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetEmployeeQuery, EmployeeResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<EmployeeResponseDto> Handle(GetEmployeeQuery query, CancellationToken cancellationToken)
        {
            return await _unitOfWork.EmployeeManager.AsNoTracking().Select(x => new EmployeeResponseDto
            {
                EmployeeId = x.EmployeeId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                DateOfBirth = x.DateOfBirth,
                DateOfJoining = x.DateOfJoining,
                DateOfResignation = x.DateOfResignation,
                Address = x.Address,
                City = x.City,
                State = x.State,
                Country = x.Country,
                PostalCode = x.PostalCode,
                IsActive = x.IsActive,
                Department = null, // TO:DO implement gRPC to get information from dpt service
                Designation = null, // TO:DO implement gRPC to get information from dgi service,
            }).FirstOrDefaultAsync(x => x.EmployeeId == query.EmployeeId, cancellationToken)
            ?? throw new NotFoundException("Employee does not exist.");
        }
    }
}
