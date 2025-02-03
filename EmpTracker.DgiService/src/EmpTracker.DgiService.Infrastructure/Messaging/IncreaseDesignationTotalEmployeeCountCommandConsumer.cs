using EmpTracker.DgiService.Application.Exceptions;
using EmpTracker.DgiService.Application.Features.Designations.Commands;
using EmpTracker.EventBus.Contracts;
using MassTransit;
using MediatR;

namespace EmpTracker.DgiService.Infrastructure.Messaging
{
    public class IncreaseDesignationTotalEmployeeCountCommandConsumer(IMediator mediator, IPublishEndpoint _publishEndpoint) : IConsumer<IncreaseDesignationTotalEmployeeCount>
    {
        private readonly IMediator _mediator = mediator;

        public async Task Consume(ConsumeContext<IncreaseDesignationTotalEmployeeCount> context)
        {
            try
            {
                await _mediator.Send(new UpdateDesignationEmployeeCountCommand(context.Message.DesignationId, 1), context.CancellationToken);
                await _publishEndpoint.Publish(new DesignationTotalEmployeeCountIncreased(context.Message.EmployeeId, context.Message.DesignationId), context.CancellationToken);
            }
            catch (NotFoundException)
            {
                await _publishEndpoint.Publish(new DesignationTotalEmployeeCountIncreased(context.Message.EmployeeId, context.Message.DesignationId), context.CancellationToken);
            }
            catch (Exception)
            {
                await _publishEndpoint.Publish(new DesignationTotalEmployeeCountIncrementFailed(context.Message.EmployeeId, context.Message.DesignationId), context.CancellationToken);
            }
        }
    }
}
