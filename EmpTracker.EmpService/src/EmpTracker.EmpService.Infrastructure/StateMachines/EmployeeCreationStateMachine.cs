using EmpTracker.EmpService.Core.Domain.SagaState;
using EmpTracker.EventBus.Contracts;
using Hangfire;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace EmpTracker.EmpService.Infrastructure.StateMachines
{
    public class EmployeeCreationStateMachine : MassTransitStateMachine<EmployeeCreationState>
    {
        private readonly IServiceProvider _serviceProvider;

        public State WaitingUserForEmployeeCreation { get; private set; }
        public State WaitingCreatedEmployeeDeletion { get; private set; }
        public State UserForEmployeeCreationSuccess { get; private set; }

        public Event<EmployeeCreationSuccess> EmployeeCreationSuccess { get; private set; }
        public Event<UserForEmployeeCreated> UserForEmployeeCreated { get; private set; }
        public Event<UserForEmployeeCreationFailed> UserForEmployeeCreationFailed { get; private set; }
        public Event<DepartmentTotalEmployeeCountIncreased> DepartmentTotalEmployeeCountIncreased { get; private set; }
        public Event<DepartmentTotalEmployeeCountIncrementFailed> DepartmentTotalEmployeeCountIncrementFailed { get; private set; }
        public Event<DesignationTotalEmployeeCountIncreased> DesignationTotalEmployeeCountIncreased { get; private set; }
        public Event<DesignationTotalEmployeeCountIncrementFailed> DesignationTotalEmployeeCountIncrementFailed { get; private set; }
        public Event<CreatedEmployeeDeleted> CreatedEmployeeDeleted { get; private set; }
        public Event<CreatedEmployeeDeletionFailed> CreatedEmployeeDeletionFailed { get; private set; }

        public EmployeeCreationStateMachine(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            InstanceState(x => x.CurrentState);

            Event(() => EmployeeCreationSuccess, e => e.CorrelateById(m => m.Message.EmployeeId));
            Event(() => UserForEmployeeCreated, e => e.CorrelateById(m => m.Message.EmployeeId));
            Event(() => UserForEmployeeCreationFailed, e => e.CorrelateById(m => m.Message.EmployeeId));
            Event(() => DepartmentTotalEmployeeCountIncreased, e => e.CorrelateById(m => m.Message.EmployeeId));
            Event(() => DepartmentTotalEmployeeCountIncrementFailed, e => e.CorrelateById(m => m.Message.EmployeeId));
            Event(() => DesignationTotalEmployeeCountIncreased, e => e.CorrelateById(m => m.Message.EmployeeId));
            Event(() => DesignationTotalEmployeeCountIncrementFailed, e => e.CorrelateById(m => m.Message.EmployeeId));
            Event(() => CreatedEmployeeDeleted, e => e.CorrelateById(m => m.Message.EmployeeId));
            Event(() => CreatedEmployeeDeletionFailed, e => e.CorrelateById(m => m.Message.EmployeeId));

            Initially(
                When(EmployeeCreationSuccess)
                    .Then(context => HandleEmployeeCreationSuccess(context))
            );

            During(WaitingUserForEmployeeCreation,
                When(UserForEmployeeCreationFailed)
                    .Then(context => HandleUserForEmployeeCreationFailed(context)),
                When(UserForEmployeeCreated)
                    .Then(context => HandleUserForEmployeeCreated(context))
            );

            During(WaitingCreatedEmployeeDeletion,
                When(CreatedEmployeeDeletionFailed)
                    .Then(context => RetryAsync(context.Saga, ++context.Saga.EmployeeRetryCount, RetryCreatedEmployeeDeletion)),
                When(CreatedEmployeeDeleted)
                    .Finalize()
            );

            During(UserForEmployeeCreationSuccess,
                When(DepartmentTotalEmployeeCountIncrementFailed)
                    .Then(context => RetryAsync(context.Saga, ++context.Saga.DepartmentRetryCount, RetryDepartmentTotalEmployeeCountIncrement)),
                When(DesignationTotalEmployeeCountIncrementFailed)
                    .Then(context => RetryAsync(context.Saga, ++context.Saga.DesignationRetryCount, RetryDesignationTotalEmployeeCountIncrement)),
                When(DepartmentTotalEmployeeCountIncreased)
                    .Then(context => HandleEmployeeCountIncremented(context, "Department")),
                When(DesignationTotalEmployeeCountIncreased)
                    .Then(context => HandleEmployeeCountIncremented(context, "Designation"))
            );
        }

        private void HandleEmployeeCreationSuccess(BehaviorContext<EmployeeCreationState, EmployeeCreationSuccess> context)
        {
            context.Saga.CorrelationId = context.Message.EmployeeId;
            context.Saga.EmployeeId = context.Message.EmployeeId;
            context.Saga.Email = context.Message.Email;
            context.Saga.DepartmentId = context.Message.DepartmentId;
            context.Saga.DesignationId = context.Message.DesignationId;

            HandleTransitionAndPublish(context, WaitingUserForEmployeeCreation, new CreateUserForEmployee(context.Message.EmployeeId, context.Message.Email));
        }

        private void HandleUserForEmployeeCreationFailed(BehaviorContext<EmployeeCreationState, UserForEmployeeCreationFailed> context)
        {
            if (context.Message.Retry)
            {
                RetryAsync(context.Saga, ++context.Saga.IdentityRetryCount, RetryUserForEmployeeCreation);
            }
            else
            {
                HandleTransitionAndPublish(context, WaitingCreatedEmployeeDeletion, new DeleteCreatedEmployee(context.Saga.EmployeeId));
            }
        }

        private void HandleUserForEmployeeCreated(BehaviorContext<EmployeeCreationState, UserForEmployeeCreated> context)
        {
            if (context.Saga.DepartmentId.HasValue)
            {
                context.Publish(new IncreaseDepartmentTotalEmployeeCount(context.Saga.EmployeeId, context.Saga.DepartmentId.Value));
            }
            else
            {
                context.Saga.DepartmentTotalEmployeeCountIncreased = true;
            }

            if (context.Saga.DesignationId.HasValue)
            {
                context.Publish(new IncreaseDesignationTotalEmployeeCount(context.Saga.EmployeeId, context.Saga.DesignationId.Value));
            }
            else
            {
                context.Saga.DesignationTotalEmployeeCountIncreased = true;
            }

            if (context.Saga.DepartmentTotalEmployeeCountIncreased && context.Saga.DesignationTotalEmployeeCountIncreased)
            {
                context.TransitionToState(Final);
            }
            else
            {
                context.TransitionToState(UserForEmployeeCreationSuccess);
            }
        }

        private void HandleEmployeeCountIncremented(BehaviorContext<EmployeeCreationState> context, string type)
        {
            if (type == "Department")
            {
                context.Saga.DepartmentTotalEmployeeCountIncreased = true;
            }
            else if (type == "Designation")
            {
                context.Saga.DesignationTotalEmployeeCountIncreased = true;
            }

            if (context.Saga.DepartmentTotalEmployeeCountIncreased && context.Saga.DesignationTotalEmployeeCountIncreased)
            {
                context.TransitionToState(Final);
            }
        }

        private static void HandleTransitionAndPublish(BehaviorContext<EmployeeCreationState> context, State nextState, object message)
        {
            context.TransitionToState(nextState);
            context.Publish(message);
        }

        private static void RetryAsync(EmployeeCreationState state, int retryCount, Func<EmployeeCreationState, Task> retryAction)
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

        public async Task RetryUserForEmployeeCreation(EmployeeCreationState state) => await PublishRetryMessage(new CreateUserForEmployee(state.EmployeeId, state.Email));
        public async Task RetryCreatedEmployeeDeletion(EmployeeCreationState state) => await PublishRetryMessage(new DeleteCreatedEmployee(state.EmployeeId));
        public async Task RetryDepartmentTotalEmployeeCountIncrement(EmployeeCreationState state) => await PublishRetryMessage(new IncreaseDepartmentTotalEmployeeCount(state.EmployeeId, state.DepartmentId!.Value));
        public async Task RetryDesignationTotalEmployeeCountIncrement(EmployeeCreationState state) => await PublishRetryMessage(new IncreaseDesignationTotalEmployeeCount(state.EmployeeId, state.DesignationId!.Value));
    }
}
