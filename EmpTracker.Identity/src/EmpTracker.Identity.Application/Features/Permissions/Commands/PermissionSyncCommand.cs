using MediatR;

namespace EmpTracker.Identity.Application.Features.Permissions.Commands
{
    public class PermissionSyncCommand(string serviceName, IEnumerable<string> permissions) : IRequest
    {
        public string ServiceName { get; private set; } = serviceName;
        public IEnumerable<string> Permissions { get; private set; } = permissions;
    }
}
