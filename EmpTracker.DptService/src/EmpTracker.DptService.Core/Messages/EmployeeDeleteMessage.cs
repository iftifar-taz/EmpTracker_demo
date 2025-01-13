namespace EmpTracker.DptService.Core.Messages
{
    public class EmployeeDeleteMessage
    {
        public Guid EmployeeId { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
