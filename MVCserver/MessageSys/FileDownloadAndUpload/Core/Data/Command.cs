using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownloadAndUpload.Core.Data
{
   public class Command
    {
        public string Name { get; set; }
        public string Operation { get; set; }
        public string Condition { set; get; }
        public string Sql { set; get; }
    }
}
