﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RDLCReportServer.Model
{
    public class mm_acc_type
    {
        public int acc_type_cd { get; set; }
        public string acc_type_desc { get; set; }
        public string trans_way { get; set; }
        public string dep_loan_flag { get; set; }
        public string intt_trf_type { get; set; }
        public string rep_sch_flag { get; set; }
    }
}