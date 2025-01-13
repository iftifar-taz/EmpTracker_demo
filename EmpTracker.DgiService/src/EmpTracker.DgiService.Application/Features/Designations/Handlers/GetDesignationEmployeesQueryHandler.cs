using EmpTracker.DgiService.Application.Dtos;
using EmpTracker.DgiService.Application.Features.Designations.Queries;
using EmpTracker.DgiService.Core.Interfaces;
using MediatR;

namespace EmpTracker.DgiService.Application.Features.Designations.Handlers
{
    public class GetDesignationEmployeesQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetDesignationEmployeesQuery, IEnumerable<DesignationEmployeeResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IEnumerable<DesignationEmployeeResponseDto>> Handle(GetDesignationEmployeesQuery query, CancellationToken cancellationToken)
        {
            // TO:DO use gRPC te get data from employee
            throw new NotImplementedException();
        }
    }
}
