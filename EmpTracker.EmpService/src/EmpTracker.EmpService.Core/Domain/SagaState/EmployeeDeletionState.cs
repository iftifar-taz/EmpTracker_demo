using System.ComponentModel.DataAnnotations;
using MassTransit;

namespace EmpTracker.EmpService.Core.Domain.SagaState
{
    public class EmployeeDeletionState : SagaStateMachineInstance
    {
        [Key]
        public required Guid CorrelationId { get; set; }
        public required string CurrentState { get; set; }

        public required Guid EmployeeId { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? DesignationId { get; set; }

        public bool DepartmentTotalEmployeeCountDecreased { get; set; }
        public bool DesignationTotalEmployeeCountDecreased { get; set; }

        public int IdentityRetryCount { get; set; }
        public int DepartmentRetryCount { get; set; }
        public int DesignationRetryCount { get; set; }
    }
}
