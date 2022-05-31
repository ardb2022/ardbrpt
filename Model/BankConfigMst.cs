using System.Collections.Generic;

namespace SBWSFinanceApi.Models
{
    public class BankConfigMst
    {
        public BankConfig DbConnections { get; set; }
        public string AllowedHosts { get; set; }
        public string bankname { get; set; }

    }
}