using MediatR;

namespace EmpTracker.DgiService.Application.Features.Designations.Commands
{
    public class UpdateDesignationCommand : IRequest
    {
        public Guid DesignationId { get; set; }
        public required string DesignationName { get; set; }
        public required string DesignationKey { get; set; }
        public string? Description { get; set; }
    }
}
