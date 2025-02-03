﻿using EmpTracker.EmpService.Core.Domain.Attribures;
using EmpTracker.EventBus.Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EmpTracker.EmpService.Infrastructure.Messaging
{
    public class PermissionPublisherService : IHostedService
    {
        private IPublishEndpoint _publishEndpoint { get; set; }

        public PermissionPublisherService(IServiceScopeFactory scopeFactory)
        {
            var scope = scopeFactory.CreateScope();
            _publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var permissions = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.FullName?.StartsWith("EmpTracker.EmpService.Api") ?? false)
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => t.IsSubclassOf(typeof(ControllerBase)) && t.IsClass && !t.IsAbstract)
                .SelectMany(controller => controller.GetMethods())
                .SelectMany(method => method.GetCustomAttributes(true).OfType<PermissionRequirementAttribute>())
                .Select(permissionRequirement => permissionRequirement.Permission)
                .Distinct()
                .ToList();

            await Task.Delay(5000, cancellationToken);
            await _publishEndpoint.Publish(new PermissionsCreationStarted(Guid.NewGuid(), "employee", permissions), cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Stop Async");
            return Task.CompletedTask;
        }
    }
}
