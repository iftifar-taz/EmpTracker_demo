namespace EmpTracker.DgiService.Core.Messages
{
    public class EmployeeCreateMessage
    {
        public Guid EmployeeId { get; set; }
        public Guid DesignationId { get; set; }
    }
}
