using EmpTracker.EventBus.Contracts;
using EmpTracker.Identity.Core.Domain.SagaState;
using Hangfire;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace EmpTracker.Identity.Infrastructure.StateMachines
{
    public class PermissionSyncStateMachine : MassTransitStateMachine<PermissionSyncState>
    {
        private readonly IServiceProvider _serviceProvider;

        public State Waiting { get; private set; }
        public State Created { get; private set; }

        public Event<PermissionsCreationStarted> PermissionsCreationStarted { get; private set; }
        public Event<PermissionsCreated> PermissionsCreated { get; private set; }
        public Event<PermissionsCreationFailed> PermissionsCreationFailed { get; private set; }

        public PermissionSyncStateMachine(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            InstanceState(x => x.CurrentState);

            Event(() => PermissionsCreationStarted, e => e.CorrelateById(m => m.Message.EventId));
            Event(() => PermissionsCreated, e => e.CorrelateById(m => m.Message.EventId));
            Event(() => PermissionsCreationFailed, e => e.CorrelateById(m => m.Message.EventId));

            Initially(
                When(PermissionsCreationStarted)
                .Then(context =>
                {
                    context.Saga.ServiceName = context.Message.ServiceName;
                    context.Saga.Permissions = context.Message.Permissions;
                })
                .TransitionTo(Waiting)
                .Publish(context => new SyncPermissions(context.Message.EventId, context.Message.ServiceName, context.Message.Permissions))
            );

            During(Waiting,
                When(PermissionsCreated)
                .TransitionTo(Created)
                .Finalize()
            );

            During(Waiting,
                When(PermissionsCreationFailed)
                .Then(context =>
                {
                    var retryDelay = TimeSpan.FromSeconds(Math.Pow(2, ++context.Saga.RetryCount));
                    BackgroundJob.Schedule(() => RetryPermissionsCreation(context.Saga), retryDelay);
                })
            );
        }

        public async Task RetryPermissionsCreation(PermissionSyncState saga)
        {
            using var scope = _serviceProvider.CreateScope();
            var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
            await publishEndpoint.Publish(new SyncPermissions(saga.CorrelationId, saga.ServiceName, saga.Permissions));
        }
    }
}
