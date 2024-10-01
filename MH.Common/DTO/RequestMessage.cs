using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH.Common.DTO
{
    public class RequestMessage
    {
        public object? RequestObj { get; set; }
        public int PageRecordSize { get; set; } = 50;
        public int PageNumber { get; set; } = 0;
        public int UserID { get; set; }
        public int Skip { get { return PageNumber > 0 ? PageNumber * PageRecordSize : 0; } }
        public string? UserName { get; set; } = "";
    }
}
