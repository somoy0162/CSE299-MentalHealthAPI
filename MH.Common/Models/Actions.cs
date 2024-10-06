using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH.Common.Models
{
    public class Actions
    {
        public int ActionID { get; set; }

        public int? PermissionID { get; set; } = 0;

        public string? ActionName { get; set; }
        public string? DisplayName { get; set; }

    }
}
