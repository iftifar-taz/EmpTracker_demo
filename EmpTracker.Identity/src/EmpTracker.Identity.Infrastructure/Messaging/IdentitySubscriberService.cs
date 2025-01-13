using EmpTracker.Identity.Application.Features.Users.Commands;
using EmpTracker.Identity.Core.Constants;
using EmpTracker.Identity.Core.Interfaces;
using EmpTracker.Identity.Core.Messages;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace EmpTracker.Identity.Infrastructure.Messaging
{
    public class IdentitySubscriberService : IHostedService
    {
        private IMediator _mediator { get; set; }
        private IMessageBus _messageBus { get; set; }

        public IdentitySubscriberService(IServiceScopeFactory scopeFactory)
        {
            var scope = scopeFactory.CreateScope();
            _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            _messageBus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _messageBus.SubscribeAsync<EmployeeCreateMessage>(EmployeeCreateMessageHandler, "empTracker.direct", ExchangeType.Direct, "employee.created", "employee.create.identity.queue");
            await _messageBus.SubscribeAsync<EmployeeDeleteMessage>(EmployeeDeleteMessageHandler, "empTracker.direct", ExchangeType.Direct, "employee.deleted", "employee.delete.identity.queue");
        }

        private async Task EmployeeCreateMessageHandler(EmployeeCreateMessage message)
        {
            var command = new CreateUserForEmployeeCommand()
            {
                EmployeeId = message.EmployeeId,
                Email = message.Email,
                PasswordRaw = GlobalConstants.DefaultPassword,
                Roles = [GlobalConstants.DefaultRole],
            };
            await _mediator.Send(command);
        }

        private async Task EmployeeDeleteMessageHandler(EmployeeDeleteMessage message)
        {
            await _mediator.Send(new DeleteUserOfEmployeeCommand(message.EmployeeId));
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() => _messageBus.DisposeConnection(), cancellationToken);
        }
    }
}
