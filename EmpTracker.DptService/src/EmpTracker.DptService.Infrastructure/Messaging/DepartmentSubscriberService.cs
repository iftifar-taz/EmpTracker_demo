using EmpTracker.DptService.Application.Features.Departments.Commands;
using EmpTracker.DptService.Core.Interfaces;
using EmpTracker.DptService.Core.Messages;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace EmpTracker.DptService.Infrastructure.Messaging
{
    public class DepartmentSubscriberService : IHostedService
    {
        private IMediator _mediator { get; set; }
        private IMessageBus _messageBus { get; set; }

        public DepartmentSubscriberService(IServiceScopeFactory scopeFactory)
        {
            var scope = scopeFactory.CreateScope();
            _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            _messageBus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _messageBus.SubscribeAsync<EmployeeCreateMessage>(EmployeeCreateMessageHandler, "empTracker.direct", ExchangeType.Direct, "employee.created", "employee.create.department.queue");
            await _messageBus.SubscribeAsync<EmployeeDeleteMessage>(EmployeeDeleteMessageHandler, "empTracker.direct", ExchangeType.Direct, "employee.deleted", "employee.delete.department.queue");
        }

        private async Task EmployeeCreateMessageHandler(EmployeeCreateMessage message)
        {
            await _mediator.Send(new UpdateDepartmentEmployeeCountCommand(message.DepartmentId, 1));
        }

        private async Task EmployeeDeleteMessageHandler(EmployeeDeleteMessage message)
        {
            await _mediator.Send(new UpdateDepartmentEmployeeCountCommand(message.DepartmentId, -1));
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() => _messageBus.DisposeConnection(), cancellationToken);
        }
    }
}
