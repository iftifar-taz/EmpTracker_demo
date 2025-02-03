using MediatR;

namespace EmpTracker.DgiService.Application.Features.Designations.Commands
{
    public class CreateDesignationCommand : IRequest<Guid>
    {
        public required string DesignationName { get; set; }
        public required string DesignationKey { get; set; }
        public string? Description { get; set; }
    }
}
