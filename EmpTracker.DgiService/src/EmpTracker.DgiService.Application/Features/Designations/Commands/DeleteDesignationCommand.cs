using MediatR;

namespace EmpTracker.DgiService.Application.Features.Designations.Commands
{
    public class DeleteDesignationCommand(Guid designationId) : IRequest
    {
        public Guid DesignationId { get; private set; } = designationId;
    }
}
