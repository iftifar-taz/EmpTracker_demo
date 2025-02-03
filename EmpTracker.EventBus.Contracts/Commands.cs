namespace EmpTracker.EventBus.Contracts
{
    #region Permission Sync Commands
    public record SyncPermissions(Guid EventId, string ServiceName, List<string> Permissions);
    #endregion

    #region Employee Create Commands
    public record CreateUserForEmployee(Guid EmployeeId, string Email);
    public record DeleteCreatedEmployee(Guid EmployeeId);
    public record IncreaseDepartmentTotalEmployeeCount(Guid EmployeeId, Guid DepartmentId);
    public record IncreaseDesignationTotalEmployeeCount(Guid EmployeeId, Guid DesignationId);
    #endregion

    #region Employee Delete Commands
    public record DeleteUserForEmployee(Guid EmployeeId);
    public record DecreaseDepartmentTotalEmployeeCount(Guid EmployeeId, Guid DepartmentId);
    public record DecreaseDesignationTotalEmployeeCount(Guid EmployeeId, Guid DesignationId);
    #endregion

    #region Department Delete Commands
    public record DeleteEmployeeDepartments(Guid DepartmentId);
    #endregion

    #region Designation Delete Commands
    public record DeleteEmployeeDesignations(Guid DesignationId);
    #endregion
}
