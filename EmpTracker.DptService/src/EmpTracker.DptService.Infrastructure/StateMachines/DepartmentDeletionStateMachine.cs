using EmpTracker.DptService.Core.Domain.SagaState;
using EmpTracker.EventBus.Contracts;
using Hangfire;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace EmpTracker.DptService.Infrastructure.StateMachines
{
    public class DepartmentDeletionStateMachine : MassTransitStateMachine<DepartmentDeletionState>
    {
        private readonly IServiceProvider _serviceProvider;

        public State Waiting { get; private set; }

        public Event<DepartmentDeletionSuccess> DepartmentDeletionSuccess { get; private set; }
        public Event<EmployeeDepartmentsDeleted> EmployeeDepartmentsDeleted { get; private set; }
        public Event<EmployeeDepartmentsDeletionFailed> EmployeeDepartmentsDeletionFailed { get; private set; }

        public DepartmentDeletionStateMachine(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            InstanceState(x => x.CurrentState);

            Event(() => DepartmentDeletionSuccess, e => e.CorrelateById(m => m.Message.DepartmentId));
            Event(() => EmployeeDepartmentsDeleted, e => e.CorrelateById(m => m.Message.DepartmentId));
            Event(() => EmployeeDepartmentsDeletionFailed, e => e.CorrelateById(m => m.Message.DepartmentId));

            Initially(
                When(DepartmentDeletionSuccess)
                    .Then(context =>
                    {
                        context.Saga.CorrelationId = context.Message.DepartmentId;
                        context.Saga.DepartmentId = context.Message.DepartmentId;
                    })
                    .TransitionTo(Waiting)
                    .Publish(context => new DeleteEmployeeDepartments(context.Saga.DepartmentId))
            );

            During(Waiting,
                When(EmployeeDepartmentsDeletionFailed)
                    .Then(context =>
                    {
                        var retryDelay = TimeSpan.FromSeconds(Math.Pow(2, ++context.Saga.EmployeeRetryCount));
                        BackgroundJob.Schedule(() => PublishRetryMessage(new DeleteEmployeeDepartments(context.Saga.DepartmentId)), retryDelay);
                    }),
                When(EmployeeDepartmentsDeleted)
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
