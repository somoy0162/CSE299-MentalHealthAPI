using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH.Common.Enums
{
    public class Enums
    {
        public enum Status
        {
            Active = 1,
            Inactive = 2,
            Delete = 9,
            Temporary = 3
        }
        public enum ResponseCode
        {
            Success = 1,
            Failed = 2,
            Warning = 3
        }
        public enum ActionType
        {
            Insert = 1,
            Update = 2,
            View = 3,
            Delete = 4,
            Login = 5,
            Register = 6,
            Logout = 7,
        }
    }
}
