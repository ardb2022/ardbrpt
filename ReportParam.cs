using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RDLCReportServer
{
    public class ReportParam
    {
        public int acc_cd { get; set; }
   public string brn_cd { get; set; }
   public  DateTime from_dt { get; set; }
   public DateTime to_dt { get; set; }
  
    }
}