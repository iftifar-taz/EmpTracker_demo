using System.ComponentModel.DataAnnotations;

namespace EmpTracker.DptService.Core.Domain.Entities
{
    public class Department
    {
        [Key]
        public Guid DepartmentId { get; set; } = Guid.NewGuid();
        [MaxLength(64)]
        public required string DepartmentName { get; set; }
        [MaxLength(16)]
        public required string DepartmentKey { get; set; }
        public string? Description { get; set; }
        public int EmployeeCount { get; set; }
    }
}
