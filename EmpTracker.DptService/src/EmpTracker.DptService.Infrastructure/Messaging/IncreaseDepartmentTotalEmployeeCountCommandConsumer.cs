using EmpTracker.DptService.Application.Exceptions;
using EmpTracker.DptService.Application.Features.Departments.Commands;
using EmpTracker.EventBus.Contracts;
using MassTransit;
using MediatR;

namespace EmpTracker.DptService.Infrastructure.Messaging
{
    public class IncreaseDepartmentTotalEmployeeCountCommandConsumer(IMediator mediator, IPublishEndpoint _publishEndpoint) : IConsumer<IncreaseDepartmentTotalEmployeeCount>
    {
        private readonly IMediator _mediator = mediator;

        public async Task Consume(ConsumeContext<IncreaseDepartmentTotalEmployeeCount> context)
        {
            try
            {
                await _mediator.Send(new UpdateDepartmentEmployeeCountCommand(context.Message.DepartmentId, 1), context.CancellationToken);
                await _publishEndpoint.Publish(new DepartmentTotalEmployeeCountIncreased(context.Message.EmployeeId, context.Message.DepartmentId), context.CancellationToken);
            }
            catch (NotFoundException)
            {
                await _publishEndpoint.Publish(new DepartmentTotalEmployeeCountIncreased(context.Message.EmployeeId, context.Message.DepartmentId), context.CancellationToken);
            }
            catch (Exception)
            {
                await _publishEndpoint.Publish(new DepartmentTotalEmployeeCountIncrementFailed(context.Message.EmployeeId, context.Message.DepartmentId), context.CancellationToken);
            }
        }
    }
}
