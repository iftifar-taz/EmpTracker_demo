using EmpTracker.DgiService.Application.Dtos;
using MediatR;

namespace EmpTracker.DgiService.Application.Features.Designations.Queries
{
    public class GetDesignationQuery(Guid designationId) : IRequest<DesignationResponseDto>
    {
        public Guid DesignationId { get; private set; } = designationId;
    }
}
