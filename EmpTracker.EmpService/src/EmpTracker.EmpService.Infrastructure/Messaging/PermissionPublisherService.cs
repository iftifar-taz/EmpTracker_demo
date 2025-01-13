using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmpTracker.EmpService.Core.Domain.Attribures;
using EmpTracker.EmpService.Core.Interfaces;
using EmpTracker.EmpService.Core.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace EmpTracker.EmpService.Infrastructure.Messaging
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
            var assembly = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName?.StartsWith("EmpTracker.EmpService.Api") ?? false);
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
                ServiceName = "department",
                Permissions = message.Distinct().ToList(),
            };

            await _messageBus.PublishAsync(dto, "empTracker.direct", ExchangeType.Direct, "identity.permission.emloyee");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() => _messageBus.DisposeConnection(), cancellationToken);
        }
    }
}
