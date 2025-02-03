using EmpTracker.DgiService.Core.Domain.SagaState;
using EmpTracker.EventBus.Contracts;
using Hangfire;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace EmpTracker.DgiService.Infrastructure.StateMachines
{
    public class DesignationDeletionStateMachine : MassTransitStateMachine<DesignationDeletionState>
    {
        private readonly IServiceProvider _serviceProvider;

        public State Waiting { get; private set; }

        public Event<DesignationDeletionSuccess> DesignationDeletionSuccess { get; private set; }
        public Event<EmployeeDesignationsDeleted> EmployeeDesignationsDeleted { get; private set; }
        public Event<EmployeeDesignationsDeletionFailed> EmployeeDesignationsDeletionFailed { get; private set; }

        public DesignationDeletionStateMachine(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            InstanceState(x => x.CurrentState);

            Event(() => DesignationDeletionSuccess, e => e.CorrelateById(m => m.Message.DesignationId));
            Event(() => EmployeeDesignationsDeleted, e => e.CorrelateById(m => m.Message.DesignationId));
            Event(() => EmployeeDesignationsDeletionFailed, e => e.CorrelateById(m => m.Message.DesignationId));

            Initially(
                When(DesignationDeletionSuccess)
                    .Then(context =>
                    {
                        context.Saga.CorrelationId = context.Message.DesignationId;
                        context.Saga.DesignationId = context.Message.DesignationId;
                    })
                    .TransitionTo(Waiting)
                    .Publish(context => new DeleteEmployeeDesignations(context.Saga.DesignationId))
            );

            During(Waiting,
                When(EmployeeDesignationsDeletionFailed)
                    .Then(context =>
                    {
                        var retryDelay = TimeSpan.FromSeconds(Math.Pow(2, ++context.Saga.EmployeeRetryCount));
                        BackgroundJob.Schedule(() => PublishRetryMessage(new DeleteEmployeeDesignations(context.Saga.DesignationId)), retryDelay);
                    }),
                When(EmployeeDesignationsDeleted)
                    .Finalize()
            );
        }

        private async Task PublishRetryMessage(object message)
        {
            using var scope = _serviceProvider.CreateScope();
            var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
            await publishEndpoint.Publish(message);
        }
    }
}
