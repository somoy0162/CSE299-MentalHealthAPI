using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH.Common.VM
{
    public class VMLogin
    {
        public string? UserName { get; set; } = "";
        public string? Password { get; set; }
        public string? Token { get; set; }
        public int SystemUserID { get; set; } = 0;
        public int? Role { get; set; }
    }
}
