using EmpTracker.DptService.Application.Dtos;
using EmpTracker.DptService.Application.Features.Departments.Queries;
using EmpTracker.DptService.Core.Interfaces;
using MediatR;

namespace EmpTracker.DptService.Application.Features.Departments.Handlers
{
    public class GetDepartmentEmployeesQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetDepartmentEmployeesQuery, IEnumerable<DepartmentEmployeeResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IEnumerable<DepartmentEmployeeResponseDto>> Handle(GetDepartmentEmployeesQuery query, CancellationToken cancellationToken)
        {
            // TO:DO use gRPC te get data from employee
            throw new NotImplementedException();
        }
    }
}
