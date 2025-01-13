using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpTracker.EmpService.Core.Messages
{
    public class PermissionSyncMessage
    {
        public string ServiceName { get; set; } = string.Empty;
        public IEnumerable<string>? Permissions { get; set; }
    }
}
