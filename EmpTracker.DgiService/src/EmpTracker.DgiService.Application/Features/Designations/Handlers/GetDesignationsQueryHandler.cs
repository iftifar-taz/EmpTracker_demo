using EmpTracker.DgiService.Application.Dtos;
using EmpTracker.DgiService.Application.Features.Designations.Queries;
using EmpTracker.DgiService.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmpTracker.DgiService.Application.Features.Designations.Handlers
{
    public class GetDesignationsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetDesignationsQuery, IEnumerable<DesignationResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IEnumerable<DesignationResponseDto>> Handle(GetDesignationsQuery query, CancellationToken cancellationToken)
        {
            return await _unitOfWork.DesignationManager.AsNoTracking().Select(x => new DesignationResponseDto
            {
                DesignationId = x.DesignationId,
                DesignationName = x.DesignationName,
                DesignationKey = x.DesignationKey,
                Description = x.Description,
                EmployeeCount = x.EmployeeCount,
            }).ToListAsync(cancellationToken);
        }
    }
}
