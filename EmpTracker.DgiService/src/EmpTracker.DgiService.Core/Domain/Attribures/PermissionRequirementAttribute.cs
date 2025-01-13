using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpTracker.DgiService.Core.Domain.Attribures
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PermissionRequirementAttribute(string permission) : Attribute
    {
        public string Permission { get; } = permission;
    }
}
