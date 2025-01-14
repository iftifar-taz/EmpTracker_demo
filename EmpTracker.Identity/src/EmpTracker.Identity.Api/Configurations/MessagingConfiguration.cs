using EmpTracker.Identity.Core.Interfaces;
using EmpTracker.Identity.Infrastructure.Messaging;

namespace EmpTracker.Identity.Api.Configurations
{
    public static class MessagingConfiguration
    {
        public static IServiceCollection ConfigureMessageBus(this IServiceCollection services)
        {
            services.AddSingleton<IMessageBus, MessageBus>();

            services.AddHostedService<IdentitySubscriberService>();
            services.AddHostedService<PermissionSubscriberService>();

            services.AddHostedService<PermissionPublisherService>();

            return services;
        }
    }
}
