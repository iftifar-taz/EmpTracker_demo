namespace EmpTracker.DptService.Core.Messages
{
    public class EmployeeCreateMessage
    {
        public Guid EmployeeId { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
