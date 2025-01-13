using EmpTracker.EmpService.Application.Features.Employees.Commands;
using EmpTracker.EmpService.Core.Interfaces;
using EmpTracker.EmpService.Core.Messages;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace EmpTracker.EmpService.Infrastructure.Messaging
{
    public class EmployeeSubscriberService : IHostedService
    {
        private IMediator _mediator { get; set; }
        private IMessageBus _messageBus { get; set; }

        public EmployeeSubscriberService(IServiceScopeFactory scopeFactory)
        {
            var scope = scopeFactory.CreateScope();
            _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            _messageBus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _messageBus.SubscribeAsync<DesignationDeleteMessage>(DesignationDeleteMessageHandler, "empTracker.direct", ExchangeType.Direct, "designation.deleted", "designation.delete.queue");
            await _messageBus.SubscribeAsync<DepartmentDeleteMessage>(DepartmentDeleteMessageHandler, "empTracker.direct", ExchangeType.Direct, "department.deleted", "department.delete.queue");
        }

        private async Task DepartmentDeleteMessageHandler(DepartmentDeleteMessage message)
        {
            await _mediator.Send(new DeleteEmployeeDepartmentsCommand(message.DepartmentId));
        }

        private async Task DesignationDeleteMessageHandler(DesignationDeleteMessage message)
        {
            await _mediator.Send(new DeleteEmployeeDesignationsCommand(message.DesignationId));
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() => _messageBus.DisposeConnection(), cancellationToken);
        }
    }
}
