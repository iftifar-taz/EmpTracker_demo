using MediatR;

namespace EmpTracker.EmpService.Application.Features.Employees.Commands
{
    public class DeleteEmployeeDesignationsCommand(Guid designationId) : IRequest
    {
        public Guid DesignationId { get; private set; } = designationId;
    }
}
