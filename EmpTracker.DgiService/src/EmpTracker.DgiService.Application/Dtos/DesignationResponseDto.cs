namespace EmpTracker.DgiService.Application.Dtos
{
    public class DesignationResponseDto
    {
        public Guid DesignationId { get; set; }
        public string DesignationName { get; set; } = string.Empty;
        public string DesignationKey { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int EmployeeCount { get; set; }
    }
}
