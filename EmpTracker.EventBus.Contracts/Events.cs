namespace EmpTracker.EventBus.Contracts
{
    #region Permission Sync Events
    public record PermissionsCreationStarted(Guid EventId, string ServiceName, List<string> Permissions);
    public record PermissionsCreated(Guid EventId, string ServiceName, List<string> Permissions);
    public record PermissionsCreationFailed(Guid EventId, string ServiceName, List<string> Permissions);
    #endregion

    #region Employee Create Events
    public record EmployeeCreationSuccess(Guid EmployeeId, string Email, Guid? DepartmentId, Guid? DesignationId);
    public record UserForEmployeeCreated(string UserId, Guid EmployeeId, string Email);
    public record UserForEmployeeCreationFailed(Guid EmployeeId, string Email, bool Retry);
    public record CreatedEmployeeDeleted(Guid EmployeeId);
    public record CreatedEmployeeDeletionFailed(Guid EmployeeId);
    public record DepartmentTotalEmployeeCountIncreased(Guid EmployeeId, Guid DepartmentId);
    public record DepartmentTotalEmployeeCountIncrementFailed(Guid EmployeeId, Guid DepartmentId);
    public record DesignationTotalEmployeeCountIncreased(Guid EmployeeId, Guid DesignationId);
    public record DesignationTotalEmployeeCountIncrementFailed(Guid EmployeeId, Guid DesignationId);
    #endregion

    #region Employee Delete Events
    public record EmployeeDeletionSuccess(Guid EmployeeId, Guid? DepartmentId, Guid? DesignationId);
    public record UserForEmployeeDeleted(Guid EmployeeId);
    public record UserForEmployeeDeletionFailed(Guid EmployeeId);
    public record DepartmentTotalEmployeeCountDecreased(Guid EmployeeId, Guid DepartmentId);
    public record DepartmentTotalEmployeeCountDecrementFailed(Guid EmployeeId, Guid DepartmentId);
    public record DesignationTotalEmployeeCountDecreased(Guid EmployeeId, Guid DesignationId);
    public record DesignationTotalEmployeeCountDecrementFailed(Guid EmployeeId, Guid DesignationId);
    #endregion

    #region Department Delete Events
    public record DepartmentDeletionSuccess(Guid DepartmentId);
    public record EmployeeDepartmentsDeleted(Guid DepartmentId);
    public record EmployeeDepartmentsDeletionFailed(Guid DepartmentId);
    #endregion

    #region Designation Delete Events
    public record DesignationDeletionSuccess(Guid DesignationId);
    public record EmployeeDesignationsDeleted(Guid DesignationId);
    public record EmployeeDesignationsDeletionFailed(Guid DesignationId);
    #endregion
}
