using System;

namespace SBWSFinanceApi.Models
{
    public class p_report_param
    {
        public int acc_cd { get; set; }
        public string brn_cd { get; set; }
        public DateTime from_dt { get; set; }
        public DateTime to_dt { get; set; }
        public DateTime trial_dt { get; set; }
        public int pl_acc_cd { get; set; }
        public int gp_acc_cd { get; set; }
        public int ad_from_acc_cd { get; set; }
        public int ad_to_acc_cd { get; set; }
        public Int16 acc_type_cd { get; set; }
        public Int16 const_cd {get;set;}
        public string acc_num { get; set; }
        public string flag { get; set; }
        public DateTime adt_dt { get; set; }  // For Loan Detailed List
        public string loan_id { get; set; }  // For Loan Statement report
        public DateTime adt_as_on_dt { get; set; } // For Loan Sub Cash Book
    }
}