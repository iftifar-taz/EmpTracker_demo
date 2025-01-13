using MediatR;

namespace EmpTracker.EmpService.Application.Features.Employees.Commands
{
    public class UpdateEmployeeCommand : IRequest
    {
        public Guid EmployeeId { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public string? FirstName { get; set; }
        public required string LastName { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public required DateTime DateOfJoining { get; set; }
        public DateTime? DateOfResignation { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string Country { get; set; }
        public required string PostalCode { get; set; }
        public required Guid DepartmentId { get; set; }
        public required Guid DesignationId { get; set; }
    }
}
