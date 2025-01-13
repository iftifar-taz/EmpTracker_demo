﻿namespace EmpTracker.Identity.Core.Messages
{
    public class PermissionSyncMessage
    {
        public string ServiceName { get; set; } = string.Empty;
        public IEnumerable<string>? Permissions { get; set; }
    }
}
