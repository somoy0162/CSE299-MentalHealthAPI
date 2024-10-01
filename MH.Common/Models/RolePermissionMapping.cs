using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH.Common.Models
{
    public class RolePermissionMapping
    {
        public int RolePermissionMappingID { get; set; }

        public int RoleID { get; set; } = 0;

        public int PermissionID { get; set; } = 0;

    }
}
