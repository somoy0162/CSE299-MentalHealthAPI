using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH.Common.Models
{
    public class Resources
    {
        public int ID { get; set; }

        public byte[] File { get; set; }

        public string FileName { get; set; }

        public DateTime? DateUploaded { get; set; }

    }
}
