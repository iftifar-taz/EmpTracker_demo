using EmpTracker.DptService.Application.Dtos;
using EmpTracker.DptService.Application.Features.Departments.Queries;
using EmpTracker.DptService.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmpTracker.DptService.Application.Features.Departments.Handlers
{
    public class GetDepartmentsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetDepartmentsQuery, IEnumerable<DepartmentResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IEnumerable<DepartmentResponseDto>> Handle(GetDepartmentsQuery query, CancellationToken cancellationToken)
        {
            return await _unitOfWork.DepartmentManager.AsNoTracking().Select(x => new DepartmentResponseDto
            {
                DepartmentId = x.DepartmentId,
                DepartmentName = x.DepartmentName,
                DepartmentKey = x.DepartmentKey,
                Description = x.Description,
                EmployeeCount = x.EmployeeCount,
            }).ToListAsync(cancellationToken);
        }
    }
}
