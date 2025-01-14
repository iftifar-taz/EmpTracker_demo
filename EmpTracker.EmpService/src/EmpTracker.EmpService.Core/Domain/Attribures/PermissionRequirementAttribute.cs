namespace EmpTracker.EmpService.Core.Domain.Attribures
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PermissionRequirementAttribute(string permission) : Attribute
    {
        public string Permission { get; } = permission;
    }
}
