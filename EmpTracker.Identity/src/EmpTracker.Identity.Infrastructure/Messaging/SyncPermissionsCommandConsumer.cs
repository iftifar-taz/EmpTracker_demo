using EmpTracker.EventBus.Contracts;
using EmpTracker.Identity.Application.Features.Permissions.Commands;
using MassTransit;
using MediatR;

namespace EmpTracker.Identity.Infrastructure.Messaging
{
    public class SyncPermissionsCommandConsumer(IMediator mediator, IPublishEndpoint _publishEndpoint) : IConsumer<SyncPermissions>
    {
        private readonly IMediator _mediator = mediator;
        public async Task Consume(ConsumeContext<SyncPermissions> context)
        {
            try
            {
                await _mediator.Send(new PermissionSyncCommand(context.Message.ServiceName, context.Message.Permissions), context.CancellationToken);
                await _publishEndpoint.Publish(new PermissionsCreated(context.Message.EventId, context.Message.ServiceName, context.Message.Permissions!), context.CancellationToken);
            }
            catch (Exception)
            {
                await _publishEndpoint.Publish(new PermissionsCreationFailed(context.Message.EventId, context.Message.ServiceName, context.Message.Permissions), context.CancellationToken);
            }
        }
    }
}
