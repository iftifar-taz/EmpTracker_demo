using EmpTracker.DgiService.Core.Interfaces;
using EmpTracker.DgiService.Infrastructure.Messaging;

namespace EmpTracker.DgiService.Api.Configurations
{
    public static class MessagingConfiguration
    {
        public static IServiceCollection ConfigureMessageBus(this IServiceCollection services)
        {
            services.AddSingleton<IMessageBus, MessageBus>();
            services.AddHostedService<DesignationSubscriberService>();
            services.AddHostedService<PermissionPublisherService>();

            return services;
        }
    }
}
