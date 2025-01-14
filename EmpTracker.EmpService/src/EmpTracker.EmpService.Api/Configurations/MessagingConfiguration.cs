using EmpTracker.EmpService.Core.Interfaces;
using EmpTracker.EmpService.Infrastructure.Messaging;

namespace EmpTracker.EmpService.Api.Configurations
{
    public static class MessagingConfiguration
    {
        public static IServiceCollection ConfigureMessageBus(this IServiceCollection services)
        {
            services.AddSingleton<IMessageBus, MessageBus>();

            services.AddHostedService<EmployeeSubscriberService>();

            services.AddHostedService<PermissionPublisherService>();

            return services;
        }
    }
}
