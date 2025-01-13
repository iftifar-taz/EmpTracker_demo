using EmpTracker.DgiService.Application.Dtos;
using MediatR;

namespace EmpTracker.DgiService.Application.Features.Designations.Queries
{
    public class GetDesignationEmployeesQuery(Guid designationId) : IRequest<IEnumerable<DesignationEmployeeResponseDto>>
    {
        public Guid DesignationId { get; private set; } = designationId;
    }
}
