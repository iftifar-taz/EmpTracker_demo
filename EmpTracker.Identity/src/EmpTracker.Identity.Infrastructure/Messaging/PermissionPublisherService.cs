using EmpTracker.Identity.Core.Domain.Attribures;
using EmpTracker.Identity.Core.Interfaces;
using EmpTracker.Identity.Core.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace EmpTracker.Identity.Infrastructure.Messaging
{
    public class PermissionPublisherService : IHostedService
    {
        private IMessageBus _messageBus { get; set; }

        public PermissionPublisherService(IServiceScopeFactory scopeFactory)
        {
            var scope = scopeFactory.CreateScope();
            _messageBus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var message = new List<string>();
            var assembly = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName?.StartsWith("EmpTracker.Identity.Api") ?? false);
            var controllers = assembly.SelectMany(x => x.GetTypes()).Where(t => t.IsSubclassOf(typeof(ControllerBase)) && t.IsClass && !t.IsAbstract); ;
            foreach (var controller in controllers)
            {
                var methods = controller.GetMethods();
                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes(true);
                    foreach (var attribute in attributes)
                    {
                        if (attribute is PermissionRequirementAttribute permissionRequirement)
                        {
                            message.Add(permissionRequirement.Permission);
                        }

                    }
                }
            }

            var dto = new PermissionSyncMessage()
            {
                ServiceName = "identity",
                Permissions = message.Distinct().ToList(),
            };

            await Task.Delay(10000, cancellationToken);
            await _messageBus.PublishAsync(dto, "empTracker.topic", ExchangeType.Topic, "identity.permission.user");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() => _messageBus.DisposeConnection(), cancellationToken);
        }
    }
}
