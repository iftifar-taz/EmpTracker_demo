using System.ComponentModel.DataAnnotations;

namespace EmpTracker.Identity.Core.Domain.Entities
{
    public class Permission
    {
        [Key]
        public Guid PermissionId { get; set; } = Guid.NewGuid();
        [MaxLength(64)]
        public required string ServiceName { get; set; }
        [MaxLength(128)]
        public required string PermissionName { get; set; }
        [MaxLength(128)]
        public required string PermissionKey { get; set; }
    }
}
