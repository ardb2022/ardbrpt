using RDLCReportServer.DL.Deposit;
using RDLCReportServer.DL.Loan;
using RDLCReportServer.Model;
using SBWSFinanceApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RDLCReportServer.LL.Loan
{
    public class LoanLL
    {
        LoanDL _dacLoanDl = new LoanDL();
        internal List<tt_detailed_list_loan> PopulateLoanDetailedList(p_report_param prp)
        {
            return _dacLoanDl.PopulateLoanDetailedList(prp);
        }

        internal List<tm_loan_all> PopulateLoanDisburseReg(p_report_param prp)
        {
            return _dacLoanDl.PopulateLoanDisburseReg(prp);
        }

        internal List<gm_loan_trans> PopulateLoanStatement(p_report_param prp)
        {
            return _dacLoanDl.PopulateLoanStatement(prp);
        }


        internal List<gm_loan_trans> PopulateRecoveryRegister(p_report_param prp)
        {
            return _dacLoanDl.PopulateRecoveryRegister(prp);
        }

        internal List<tt_loan_sub_cash_book> PopulateLoanSubCashBook(p_report_param prp)
        {
            return _dacLoanDl.PopulateLoanSubCashBook(prp);
        }

        internal List<tt_detailed_list_loan> GetDefaultList(p_report_param prp)
        {
            return _dacLoanDl.GetDefaultList(prp);
        }

    }
}

