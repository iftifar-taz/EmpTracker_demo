namespace EmpTracker.EmpService.Application.Dtos
{
    public class EmployeeMessageRequestDto
    {
        public Guid EmployeeId { get; set; }
        public string Email { get; set; } = string.Empty;
        public Guid? DepartmentId { get; set; }
        public Guid? DesignationId { get; set; }
    }
}
