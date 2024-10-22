using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH.Common.VM
{
    public class VMForgotPassword
    {
        public int? SystemUserID { get; set; } = 0;
        public string ConfirmPassword { get; set; }
        public string CurrentPassword { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
    }
}
