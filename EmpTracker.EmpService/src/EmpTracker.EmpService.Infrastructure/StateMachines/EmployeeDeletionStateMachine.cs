using EmpTracker.EmpService.Core.Domain.SagaState;
using EmpTracker.EventBus.Contracts;
using Hangfire;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace EmpTracker.EmpService.Infrastructure.StateMachines
{
    public class EmployeeDeletionStateMachine : MassTransitStateMachine<EmployeeDeletionState>
    {
        private readonly IServiceProvider _serviceProvider;

        public State WaitingUserForEmployeeDeletion { get; private set; }
        public State UserForEmployeeDeletionSuccess { get; private set; }

        public Event<EmployeeDeletionSuccess> EmployeeDeletionSuccess { get; private set; }
        public Event<UserForEmployeeDeleted> UserForEmployeeDeleted { get; private set; }
        public Event<UserForEmployeeDeletionFailed> UserForEmployeeDeletionFailed { get; private set; }
        public Event<DepartmentTotalEmployeeCountDecreased> DepartmentTotalEmployeeCountDecreased { get; private set; }
        public Event<DepartmentTotalEmployeeCountDecrementFailed> DepartmentTotalEmployeeCountDecrementFailed { get; private set; }
        public Event<DesignationTotalEmployeeCountDecreased> DesignationTotalEmployeeCountDecreased { get; private set; }
        public Event<DesignationTotalEmployeeCountDecrementFailed> DesignationTotalEmployeeCountDecrementFailed { get; private set; }

        public EmployeeDeletionStateMachine(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            InstanceState(x => x.CurrentState);

            Event(() => EmployeeDeletionSuccess, e => e.CorrelateById(m => m.Message.EmployeeId));
            Event(() => UserForEmployeeDeleted, e => e.CorrelateById(m => m.Message.EmployeeId));
            Event(() => UserForEmployeeDeletionFailed, e => e.CorrelateById(m => m.Message.EmployeeId));
            Event(() => DepartmentTotalEmployeeCountDecreased, e => e.CorrelateById(m => m.Message.EmployeeId));
            Event(() => DepartmentTotalEmployeeCountDecrementFailed, e => e.CorrelateById(m => m.Message.EmployeeId));
            Event(() => DesignationTotalEmployeeCountDecreased, e => e.CorrelateById(m => m.Message.EmployeeId));
            Event(() => DesignationTotalEmployeeCountDecrementFailed, e => e.CorrelateById(m => m.Message.EmployeeId));

            Initially(
                When(EmployeeDeletionSuccess)
                    .Then(context => HandleEmployeeDeletionSuccess(context))
            );

            During(WaitingUserForEmployeeDeletion,
                When(UserForEmployeeDeletionFailed)
                    .Then(context => HandleUserForEmployeeDeletionFailed(context)),
                When(UserForEmployeeDeleted)
                    .Then(context => HandleUserForEmployeeDeleted(context))
            );

            During(UserForEmployeeDeletionSuccess,
                When(DepartmentTotalEmployeeCountDecrementFailed)
                    .Then(context => RetryAsync(context.Saga, ++context.Saga.DepartmentRetryCount, RetryDepartmentTotalEmployeeCountDecrement)),
                When(DesignationTotalEmployeeCountDecrementFailed)
                    .Then(context => RetryAsync(context.Saga, ++context.Saga.DesignationRetryCount, RetryDesignationTotalEmployeeCountDecrement)),
                When(DepartmentTotalEmployeeCountDecreased)
                    .Then(context => HandleEmployeeCountDecremented(context, "Department")),
                When(DesignationTotalEmployeeCountDecreased)
                    .Then(context => HandleEmployeeCountDecremented(context, "Designation"))
            );
        }

        private void HandleEmployeeDeletionSuccess(BehaviorContext<EmployeeDeletionState, EmployeeDeletionSuccess> context)
        {
            context.Saga.CorrelationId = context.Message.EmployeeId;
            context.Saga.EmployeeId = context.Message.EmployeeId;
            context.Saga.DepartmentId = context.Message.DepartmentId;
            context.Saga.DesignationId = context.Message.DesignationId;

            HandleTransitionAndPublish(context, WaitingUserForEmployeeDeletion, new DeleteUserForEmployee(context.Message.EmployeeId));
        }

        private void HandleUserForEmployeeDeletionFailed(BehaviorContext<EmployeeDeletionState, UserForEmployeeDeletionFailed> context)
        {
            RetryAsync(context.Saga, ++context.Saga.IdentityRetryCount, RetryUserForEmployeeDeletion);
        }

        private void HandleUserForEmployeeDeleted(BehaviorContext<EmployeeDeletionState, UserForEmployeeDeleted> context)
        {
            if (context.Saga.DepartmentId.HasValue)
            {
                context.Publish(new DecreaseDepartmentTotalEmployeeCount(context.Saga.EmployeeId, context.Saga.DepartmentId.Value));
            }
            else
            {
                context.Saga.DepartmentTotalEmployeeCountDecreased = true;
            }

            if (context.Saga.DesignationId.HasValue)
            {
                context.Publish(new DecreaseDesignationTotalEmployeeCount(context.Saga.EmployeeId, context.Saga.DesignationId.Value));
            }
            else
            {
                context.Saga.DesignationTotalEmployeeCountDecreased = true;
            }

            if (context.Saga.DepartmentTotalEmployeeCountDecreased && context.Saga.DesignationTotalEmployeeCountDecreased)
            {
                context.TransitionToState(Final);
            }
            else
            {
                context.TransitionToState(UserForEmployeeDeletionSuccess);
            }
        }

        private void HandleEmployeeCountDecremented(BehaviorContext<EmployeeDeletionState> context, string type)
        {
            if (type == "Department")
            {
                context.Saga.DepartmentTotalEmployeeCountDecreased = true;
            }
            else if (type == "Designation")
            {
                context.Saga.DesignationTotalEmployeeCountDecreased = true;
            }

            if (context.Saga.DepartmentTotalEmployeeCountDecreased && context.Saga.DesignationTotalEmployeeCountDecreased)
            {
                context.TransitionToState(Final);
            }
        }

        private static void HandleTransitionAndPublish(BehaviorContext<EmployeeDeletionState> context, State nextState, object message)
        {
            context.TransitionToState(nextState);
            context.Publish(message);
        }

        private static void RetryAsync(EmployeeDeletionState state, int retryCount, Func<EmployeeDeletionState, Task> retryAction)
        {
            var retryDelay = TimeSpan.FromSeconds(Math.Pow(2, retryCount));
            BackgroundJob.Schedule(() => retryAction(state), retryDelay);
        }

        private async Task PublishRetryMessage(object message)
        {
            using var scope = _serviceProvider.CreateScope();
            var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
            await publishEndpoint.Publish(message);
        }

        public async Task RetryUserForEmployeeDeletion(EmployeeDeletionState state) => await PublishRetryMessage(new DeleteUserForEmployee(state.EmployeeId));
        public async Task RetryDepartmentTotalEmployeeCountDecrement(EmployeeDeletionState state) => await PublishRetryMessage(new DecreaseDepartmentTotalEmployeeCount(state.EmployeeId, state.DepartmentId!.Value));
        public async Task RetryDesignationTotalEmployeeCountDecrement(EmployeeDeletionState state) => await PublishRetryMessage(new DecreaseDesignationTotalEmployeeCount(state.EmployeeId, state.DesignationId!.Value));
    }
}
