namespace EmpTracker.EmpService.Application.Dtos
{
    public class EmployeeResponseDto
    {
        public Guid EmployeeId { get; set; }
        public string? FirstName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfJoining { get; set; }
        public DateTime? DateOfResignation { get; set; }
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public EmployeeDepartmentResponseDto? Department { get; set; }
        public EmployeeDesignationResponseDto? Designation { get; set; }
    }
}
