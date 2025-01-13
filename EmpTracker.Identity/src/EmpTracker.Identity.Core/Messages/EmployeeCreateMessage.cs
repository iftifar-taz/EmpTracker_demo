namespace EmpTracker.Identity.Core.Messages
{
    public class EmployeeCreateMessage
    {
        public Guid EmployeeId { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
