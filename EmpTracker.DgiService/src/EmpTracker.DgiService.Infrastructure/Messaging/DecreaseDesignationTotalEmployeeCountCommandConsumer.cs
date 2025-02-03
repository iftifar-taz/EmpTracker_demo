using EmpTracker.DgiService.Application.Exceptions;
using EmpTracker.DgiService.Application.Features.Designations.Commands;
using EmpTracker.EventBus.Contracts;
using MassTransit;
using MediatR;

namespace EmpTracker.DgiService.Infrastructure.Messaging
{
    public class DecreaseDesignationTotalEmployeeCountCommandConsumer(IMediator mediator, IPublishEndpoint _publishEndpoint) : IConsumer<DecreaseDesignationTotalEmployeeCount>
    {
        private readonly IMediator _mediator = mediator;

        public async Task Consume(ConsumeContext<DecreaseDesignationTotalEmployeeCount> context)
        {
            try
            {
                await _mediator.Send(new UpdateDesignationEmployeeCountCommand(context.Message.DesignationId, -1), context.CancellationToken);
                await _publishEndpoint.Publish(new DesignationTotalEmployeeCountDecreased(context.Message.EmployeeId, context.Message.DesignationId), context.CancellationToken);
            }
            catch (NotFoundException)
            {
                await _publishEndpoint.Publish(new DesignationTotalEmployeeCountDecreased(context.Message.EmployeeId, context.Message.DesignationId), context.CancellationToken);
            }
            catch (Exception)
            {
                await _publishEndpoint.Publish(new DesignationTotalEmployeeCountDecrementFailed(context.Message.EmployeeId, context.Message.DesignationId), context.CancellationToken);
            }
        }
    }
}
