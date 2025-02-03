using EmpTracker.EventBus.Contracts;
using EmpTracker.Identity.Application.Exceptions;
using EmpTracker.Identity.Application.Features.Users.Commands;
using EmpTracker.Identity.Core.Constants;
using MassTransit;
using MediatR;

namespace EmpTracker.Identity.Infrastructure.Messaging
{
    public class CreateUserForEmployeeCommandConsumer(IMediator mediator, IPublishEndpoint _publishEndpoint) : IConsumer<CreateUserForEmployee>
    {
        private readonly IMediator _mediator = mediator;
        public async Task Consume(ConsumeContext<CreateUserForEmployee> context)
        {
            try
            {
                var command = new CreateUserForEmployeeCommand()
                {
                    EmployeeId = context.Message.EmployeeId,
                    Email = context.Message.Email,
                    PasswordRaw = GlobalConstants.DefaultPassword,
                    Roles = [GlobalConstants.DefaultRole],
                };
                var userId = await _mediator.Send(command, context.CancellationToken);
                await _publishEndpoint.Publish(new UserForEmployeeCreated(userId, command.EmployeeId, command.Email), context.CancellationToken);
            }
            catch (ConflictException)
            {
                await _publishEndpoint.Publish(new UserForEmployeeCreationFailed(context.Message.EmployeeId, context.Message.Email, false), context.CancellationToken);
            }
            catch (Exception ex) when (ex is UserCreationException or Exception)
            {
                await _publishEndpoint.Publish(new UserForEmployeeCreationFailed(context.Message.EmployeeId, context.Message.Email, true), context.CancellationToken);
            }
        }
    }
}
