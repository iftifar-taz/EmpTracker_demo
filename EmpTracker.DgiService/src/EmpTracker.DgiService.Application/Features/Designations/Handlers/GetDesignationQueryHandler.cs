using EmpTracker.DgiService.Application.Dtos;
using EmpTracker.DgiService.Application.Exceptions;
using EmpTracker.DgiService.Application.Features.Designations.Queries;
using EmpTracker.DgiService.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmpTracker.DgiService.Application.Features.Designations.Handlers
{
    public class GetDesignationQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetDesignationQuery, DesignationResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<DesignationResponseDto> Handle(GetDesignationQuery query, CancellationToken cancellationToken)
        {
            return await _unitOfWork.DesignationManager.AsNoTracking().Select(x => new DesignationResponseDto
            {
                DesignationId = x.DesignationId,
                DesignationName = x.DesignationName,
                DesignationKey = x.DesignationKey,
                Description = x.Description,
                EmployeeCount = x.EmployeeCount,
            }).FirstOrDefaultAsync(x => x.DesignationId == query.DesignationId, cancellationToken)
            ?? throw new NotFoundException("Designation does not exist.");
        }
    }
}
