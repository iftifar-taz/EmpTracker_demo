using System.ComponentModel.DataAnnotations;
using MassTransit;

namespace EmpTracker.DgiService.Core.Domain.SagaState
{
    public class DesignationDeletionState : SagaStateMachineInstance
    {
        [Key]
        public required Guid CorrelationId { get; set; }
        public required string CurrentState { get; set; }

        public required Guid DesignationId { get; set; }

        public int EmployeeRetryCount { get; set; }
    }
}
