namespace EmpTracker.DptService.Application.Dtos
{
    public class DepartmentResponseDto
    {
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string DepartmentKey { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int EmployeeCount { get; set; }
    }
}
