using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH.Common.DTO
{
    public class ResponseMessage
    {
        public object? ResponseObj { get; set; }
        public int ResponseCode { get; set; } = 0;
        public string? Message { get; set; }
        public int LastRow { get; set; } = 0;
        public int TotalCount { get; set; } = 0;
        public int IsImport { get; set; } = 0;
        public int? UserID { get; set; } = 0;
        public object SessionEnd { get; set; }
    }
}
