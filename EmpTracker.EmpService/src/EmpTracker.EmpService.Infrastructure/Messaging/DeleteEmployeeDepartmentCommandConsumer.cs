using EmpTracker.EmpService.Application.Features.Employees.Commands;
using EmpTracker.EventBus.Contracts;
using MassTransit;
using MediatR;

namespace EmpTracker.EmpService.Infrastructure.Messaging
{
    public class DeleteEmployeeDepartmentCommandConsumer(IMediator mediator, IPublishEndpoint _publishEndpoint) : IConsumer<DeleteEmployeeDepartments>
    {
        private readonly IMediator _mediator = mediator;
        public async Task Consume(ConsumeContext<DeleteEmployeeDepartments> context)
        {
            try
            {
                await _mediator.Send(new DeleteEmployeeDepartmentsCommand(context.Message.DepartmentId), context.CancellationToken);
                await _publishEndpoint.Publish(new EmployeeDepartmentsDeleted(context.Message.DepartmentId), context.CancellationToken);
            }
            catch (Exception)
            {
                await _publishEndpoint.Publish(new EmployeeDepartmentsDeletionFailed(context.Message.DepartmentId), context.CancellationToken);
            }
        }
    }
}
