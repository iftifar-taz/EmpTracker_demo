using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmpTracker.DgiService.Application.Features.Designations.Commands;
using EmpTracker.DgiService.Core.Interfaces;
using EmpTracker.DgiService.Core.Messages;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace EmpTracker.DgiService.Infrastructure.Messaging
{
    public class DesignationSubscriberService : IHostedService
    {
        private IMediator _mediator { get; set; }
        private IMessageBus _messageBus { get; set; }

        public DesignationSubscriberService(IServiceScopeFactory scopeFactory)
        {
            var scope = scopeFactory.CreateScope();
            _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            _messageBus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _messageBus.SubscribeAsync<EmployeeCreateMessage>(EmployeeCreateMessageHandler, "empTracker.direct", ExchangeType.Direct, "employee.created", "employee.create.designation.queue");
            await _messageBus.SubscribeAsync<EmployeeDeleteMessage>(EmployeeDeleteMessageHandler, "empTracker.direct", ExchangeType.Direct, "employee.deleted", "employee.delete.designation.queue");
        }

        private async Task EmployeeCreateMessageHandler(EmployeeCreateMessage message)
        {
            await _mediator.Send(new UpdateDesignationEmployeeCountCommand(message.DesignationId, 1));
        }

        private async Task EmployeeDeleteMessageHandler(EmployeeDeleteMessage message)
        {
            await _mediator.Send(new UpdateDesignationEmployeeCountCommand(message.DesignationId, -1));
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() => _messageBus.DisposeConnection(), cancellationToken);
        }
    }
}
