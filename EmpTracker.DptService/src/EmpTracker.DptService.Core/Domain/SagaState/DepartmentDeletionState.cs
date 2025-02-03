using System.ComponentModel.DataAnnotations;
using MassTransit;

namespace EmpTracker.DptService.Core.Domain.SagaState
{
    public class DepartmentDeletionState : SagaStateMachineInstance
    {
        [Key]
        public required Guid CorrelationId { get; set; }
        public required string CurrentState { get; set; }

        public required Guid DepartmentId { get; set; }

        public int EmployeeRetryCount { get; set; }
    }
}
