using EmpTracker.Identity.Application.Features.Permissions.Commands;
using EmpTracker.Identity.Application.Features.Users.Commands;
using EmpTracker.Identity.Core.Constants;
using EmpTracker.Identity.Core.Domain.Attribures;
using EmpTracker.Identity.Core.Interfaces;
using EmpTracker.Identity.Core.Messages;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace EmpTracker.Identity.Infrastructure.Messaging
{
    public class PermissionSubscriberService : IHostedService
    {
        private IMediator _mediator { get; set; }
        private IMessageBus _messageBus { get; set; }

        public PermissionSubscriberService(IServiceScopeFactory scopeFactory)
        {
            var scope = scopeFactory.CreateScope();
            _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            _messageBus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _messageBus.SubscribeAsync<PermissionSyncMessage>(PermissionSyncMessageHandler, "empTracker.direct", ExchangeType.Direct, "identity.permission.*", "identity.permissions.queue");
        }

        private async Task PermissionSyncMessageHandler(PermissionSyncMessage message)
        {
            await _mediator.Send(new PermissionSyncCommand(message.ServiceName, message.Permissions));
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() => _messageBus.DisposeConnection(), cancellationToken);
        }
    }
}
