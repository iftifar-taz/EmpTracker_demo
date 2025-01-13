using EmpTracker.Identity.Core.Interfaces;
using EmpTracker.Identity.Infrastructure.Messaging;
using Microsoft.OpenApi.Models;

namespace EmpTracker.Identity.Api.Configurations
{
    public static class MessagingConfiguration
    {
        public static IServiceCollection ConfigureMessageBus(this IServiceCollection services)
        {
            services.AddSingleton<IMessageBus, MessageBus>();
            services.AddHostedService<PermissionPublisherService>();
            services.AddHostedService<IdentitySubscriberService>();
            services.AddHostedService<PermissionSubscriberService>();

            return services;
        }
    }
}
