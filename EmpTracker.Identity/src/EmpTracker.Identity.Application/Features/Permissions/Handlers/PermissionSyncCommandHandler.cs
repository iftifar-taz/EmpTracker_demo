using EmpTracker.Identity.Application.Extentions;
using EmpTracker.Identity.Application.Features.Permissions.Commands;
using EmpTracker.Identity.Core.Domain.Entities;
using EmpTracker.Identity.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmpTracker.Identity.Application.Features.Permissions.Handlers
{
    public class PermissionSyncCommandHandler(IUnitOfWork unitOfWork, ILogger<PermissionSyncCommandHandler> logger) : IRequestHandler<PermissionSyncCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<PermissionSyncCommandHandler> _logger = logger;

        public async Task Handle(PermissionSyncCommand command, CancellationToken cancellationToken)
        {
            var existingPermissions = await _unitOfWork.PermissionManager.Select(x => x.PermissionKey).ToListAsync(cancellationToken);
            var newPermissions = command.Permissions.Where(x => !existingPermissions.Contains(x));
            var toSaveList = newPermissions.Select(x => new Permission
            {
                PermissionName = x.ToPermissionName(),
                ServiceName = command.ServiceName,
                PermissionKey = x
            });
            await _unitOfWork.PermissionManager.AddRangeAsync(toSaveList, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
            if (toSaveList.Any())
            {
                _logger.LogInformation("New permissions added");
            }
        }
    }
}
