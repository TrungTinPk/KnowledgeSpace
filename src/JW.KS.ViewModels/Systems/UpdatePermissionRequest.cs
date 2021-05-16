using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JW.KS.ViewModels.Systems
{
    public class UpdatePermissionRequest
    {
        public List<PermissionVm> Permissions { get; set; } = new List<PermissionVm>();
    }
}
