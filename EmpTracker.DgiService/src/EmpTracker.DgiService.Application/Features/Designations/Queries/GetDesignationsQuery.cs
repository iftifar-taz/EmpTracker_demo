using EmpTracker.DgiService.Application.Dtos;
using MediatR;

namespace EmpTracker.DgiService.Application.Features.Designations.Queries
{
    public class GetDesignationsQuery() : IRequest<IEnumerable<DesignationResponseDto>>
    {
    }
}
