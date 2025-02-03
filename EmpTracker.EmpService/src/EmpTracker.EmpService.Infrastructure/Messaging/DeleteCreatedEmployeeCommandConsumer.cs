using EmpTracker.EmpService.Application.Exceptions;
using EmpTracker.EmpService.Application.Features.Employees.Commands;
using EmpTracker.EventBus.Contracts;
using MassTransit;
using MediatR;

namespace EmpTracker.EmpService.Infrastructure.Messaging
{
    public class DeleteCreatedEmployeeCommandConsumer(IMediator mediator, IPublishEndpoint _publishEndpoint) : IConsumer<CreateUserForEmployee>
    {
        private readonly IMediator _mediator = mediator;
        public async Task Consume(ConsumeContext<CreateUserForEmployee> context)
        {
            try
            {
                await _mediator.Send(new DeleteEmployeeCommand(context.Message.EmployeeId), context.CancellationToken);
                await _publishEndpoint.Publish(new CreatedEmployeeDeleted(context.Message.EmployeeId), context.CancellationToken);
            }
            catch (NotFoundException)
            {
                await _publishEndpoint.Publish(new CreatedEmployeeDeleted(context.Message.EmployeeId), context.CancellationToken);
            }
            catch (Exception)
            {
                await _publishEndpoint.Publish(new CreatedEmployeeDeletionFailed(context.Message.EmployeeId), context.CancellationToken);
            }
        }
    }
}
