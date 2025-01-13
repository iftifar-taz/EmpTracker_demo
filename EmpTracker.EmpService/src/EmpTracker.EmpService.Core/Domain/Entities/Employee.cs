using System.ComponentModel.DataAnnotations;

namespace EmpTracker.EmpService.Core.Domain.Entities
{
    public class Employee
    {
        [Key]
        public Guid EmployeeId { get; set; } = Guid.NewGuid();
        [MaxLength(64)]
        public string? FirstName { get; set; }
        [MaxLength(64)]
        public required string LastName { get; set; }
        [MaxLength(64)]
        public required string Email { get; set; }
        [MaxLength(64)]
        public required string PhoneNumber { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public required DateTime DateOfJoining { get; set; }
        public DateTime? DateOfResignation { get; set; }
        [MaxLength(256)]
        public required string Address { get; set; }
        [MaxLength(32)]
        public required string City { get; set; }
        [MaxLength(32)]
        public required string State { get; set; }
        [MaxLength(32)]
        public required string Country { get; set; }
        [MaxLength(16)]
        public required string PostalCode { get; set; }
        public required bool IsActive { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? DesignationId { get; set; }
    }
}
