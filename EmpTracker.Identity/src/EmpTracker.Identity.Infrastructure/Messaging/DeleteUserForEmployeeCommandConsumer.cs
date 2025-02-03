using EmpTracker.EventBus.Contracts;
using EmpTracker.Identity.Application.Exceptions;
using EmpTracker.Identity.Application.Features.Users.Commands;
using MassTransit;
using MediatR;

namespace EmpTracker.Identity.Infrastructure.Messaging
{
    public class DeleteUserForEmployeeCommandConsumer(IMediator mediator, IPublishEndpoint _publishEndpoint) : IConsumer<DeleteUserForEmployee>
    {
        private readonly IMediator _mediator = mediator;
        public async Task Consume(ConsumeContext<DeleteUserForEmployee> context)
        {
            try
            {
                await _mediator.Send(new DeleteUserForEmployeeCommand(context.Message.EmployeeId), context.CancellationToken);
                await _publishEndpoint.Publish(new UserForEmployeeDeleted(context.Message.EmployeeId), context.CancellationToken);
            }
            catch (NotFoundException)
            {
                await _publishEndpoint.Publish(new UserForEmployeeDeleted(context.Message.EmployeeId), context.CancellationToken);
            }
            catch (Exception ex) when (ex is UserCreationException or Exception)
            {
                await _publishEndpoint.Publish(new UserForEmployeeDeletionFailed(context.Message.EmployeeId), context.CancellationToken);
            }
        }
    }
}
