using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH.Common.Constants
{
    public class Constants
    {
        public const string Token = "authorization";
        public const string AuthenticationSchema = "Bearer";
    }

    public static class CommonConstant
    {
        public static DateTime DeafultDate = Convert.ToDateTime("1900/01/01");
        public static int SessionExpired = 30;
        public static string DateFormate = "yyyy/MM/dd";
    }
}
