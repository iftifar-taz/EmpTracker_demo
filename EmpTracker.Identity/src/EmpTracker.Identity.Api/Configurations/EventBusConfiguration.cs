﻿using EmpTracker.Identity.Core.Domain.SagaState;
using EmpTracker.Identity.Infrastructure.Messaging;
using EmpTracker.Identity.Infrastructure.Persistence;
using EmpTracker.Identity.Infrastructure.StateMachines;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace EmpTracker.Identity.Api.Configurations
{
    public static class EventBusConfiguration
    {
        public static IServiceCollection ConfigureEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SagaContext>(builder =>
                builder.UseSqlServer(configuration.GetConnectionString("Default"), m =>
                {
                    m.MigrationsAssembly(typeof(SagaContext).Assembly.GetName().Name);
                    m.MigrationsHistoryTable($"__{nameof(SagaContext)}_EFMigrationsHistory", "saga");
                }));

            services.AddMassTransit(x =>
            {
                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("emp-tracker-"));
                x.AddDelayedMessageScheduler();
                x.AddConsumers(typeof(SyncPermissionsCommandConsumer).Assembly);

                x.AddConfigureEndpointsCallback((context, _, cfg) =>
                {
                    cfg.UseMessageRetry(r => r.Intervals(2, 10, 60, 300, 1800));
                    cfg.UseMessageScope(context);
                    cfg.UseInMemoryOutbox(context);
                });

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["RabbitMq:Host"], h =>
                    {
                        h.Username(configuration["RabbitMq:Username"]!);
                        h.Password(configuration["RabbitMq:Password"]!);
                    });
                    cfg.UseDelayedMessageScheduler();
                    cfg.ConfigureEndpoints(context);
                });

                x.AddSagaStateMachine<PermissionSyncStateMachine, PermissionSyncState>()
                   .EntityFrameworkRepository(r =>
                   {
                       r.ExistingDbContext<SagaContext>();
                       r.UseSqlServer();
                   });
            });

            services.AddHostedService<PermissionPublisherService>();

            return services;
        }
    }
}
