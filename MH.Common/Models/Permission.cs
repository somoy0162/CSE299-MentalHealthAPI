using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH.Common.Models
{
    public class Permission
    {
        public int PermissionID { get; set; }
        public string? PermissionName { get; set; }
        public string? DisplayName { get; set; }

        [NotMapped]
        public List<Actions>? Actions { get; set; }

    }
}
