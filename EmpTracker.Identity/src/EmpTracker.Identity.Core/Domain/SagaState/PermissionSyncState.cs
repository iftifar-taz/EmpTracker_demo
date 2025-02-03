using System.ComponentModel.DataAnnotations;
using MassTransit;

namespace EmpTracker.Identity.Core.Domain.SagaState
{
    public class PermissionSyncState : SagaStateMachineInstance
    {
        [Key]
        public required Guid CorrelationId { get; set; }
        public required string CurrentState { get; set; }

        public required string ServiceName { get; set; }
        public List<string> Permissions { get; set; } = [];
        public int RetryCount { get; set; }
    }
}
