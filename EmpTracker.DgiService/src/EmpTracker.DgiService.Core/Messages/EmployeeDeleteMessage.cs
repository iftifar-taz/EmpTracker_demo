namespace EmpTracker.DgiService.Core.Messages
{
    public class EmployeeDeleteMessage
    {
        public Guid EmployeeId { get; set; }
        public Guid DesignationId { get; set; }
    }
}
