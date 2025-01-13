using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpTracker.DgiService.Core.Messages
{
    public class EmployeeCreateMessage
    {
        public Guid EmployeeId { get; set; }
        public Guid DesignationId { get; set; }
    }
}
