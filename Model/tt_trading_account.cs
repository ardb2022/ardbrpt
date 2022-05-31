using System;

namespace SBWSFinanceApi.Models
{
    public class tt_trading_account
    {
        public string acc_type { get; set; }
        public int acc_cd { get; set; }
        public int sub_schedule_cd { get; set; }
        public decimal amount { get; set; }
        public int schedule_cd { get; set; }
        public string acc_name { get; set; }
        public string type { get; set; }
        public string trading_flag {get; set;}
    }
}