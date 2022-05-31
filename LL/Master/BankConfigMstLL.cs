using RDLCReportServer.Model;
using SBWSFinanceApi.DL;
using SBWSFinanceApi.Models;
using System.Collections.Generic;

namespace SBWSFinanceApi.LL
{
    internal class BankConfigMstLL
    {
        internal BankConfigMst ReadAllConfiguration()
        {
            BankConfigMstDL obj = new BankConfigMstDL();
            return obj.ReadAllConfiguration();
        }

        public string GetBranchMaster(string brn_cd)
        {
            BankConfigMstDL obj = new BankConfigMstDL();
            return obj.GetBranchMaster(brn_cd);
        }

        public List<mm_category> GetCategoryMaster()
        {
            BankConfigMstDL obj = new BankConfigMstDL();
            return obj.GetCategoryMaster();
        }
        public List<mm_acc_type> GetAccountTypeMaster()
        {
            BankConfigMstDL obj = new BankConfigMstDL();
            return obj.GetAccountTypeMaster();
        }
        public List<m_acc_master> GetAccountMaster()
        {
            BankConfigMstDL obj = new BankConfigMstDL();
            return obj.GetAccountMaster();
        }

        public List<mm_constitution> GetConstitution()
        {
            BankConfigMstDL obj = new BankConfigMstDL();
            return obj.GetConstitution();
        }

    }
}