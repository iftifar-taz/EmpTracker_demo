using EmpTracker.DptService.Core.Interfaces;
using EmpTracker.DptService.Infrastructure.Messaging;

namespace EmpTracker.DptService.Api.Configurations
{
    public static class MessagingConfiguration
    {
        public static IServiceCollection ConfigureMessageBus(this IServiceCollection services)
        {
            services.AddSingleton<IMessageBus, MessageBus>();
            services.AddHostedService<DepartmentSubscriberService>();
            services.AddHostedService<PermissionPublisherService>();

            return services;
        }
    }
}
