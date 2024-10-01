using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH.Common.Models
{
    public class ForgotPasswordToken
    {
        public int ForgotPassWordID { get; set; }
        public int? SystemUserID { get; set; }
        public string VerifiedToken { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool? IsVerified { get; set; }
    }
}
