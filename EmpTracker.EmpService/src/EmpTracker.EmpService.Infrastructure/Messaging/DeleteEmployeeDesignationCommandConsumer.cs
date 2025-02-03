using EmpTracker.EmpService.Application.Features.Employees.Commands;
using EmpTracker.EventBus.Contracts;
using MassTransit;
using MediatR;

namespace EmpTracker.EmpService.Infrastructure.Messaging
{
    public class DeleteEmployeeDesignationCommandConsumer(IMediator mediator, IPublishEndpoint _publishEndpoint) : IConsumer<DeleteEmployeeDesignations>
    {
        private readonly IMediator _mediator = mediator;
        public async Task Consume(ConsumeContext<DeleteEmployeeDesignations> context)
        {
            try
            {
                await _mediator.Send(new DeleteEmployeeDesignationsCommand(context.Message.DesignationId), context.CancellationToken);
                await _publishEndpoint.Publish(new EmployeeDesignationsDeleted(context.Message.DesignationId), context.CancellationToken);
            }
            catch (Exception)
            {
                await _publishEndpoint.Publish(new EmployeeDesignationsDeletionFailed(context.Message.DesignationId), context.CancellationToken);
            }
        }
    }
}
