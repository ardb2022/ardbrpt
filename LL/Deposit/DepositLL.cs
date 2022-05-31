using RDLCReportServer.DL.Deposit;
using RDLCReportServer.Model;
using SBWSFinanceApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RDLCReportServer.LL.Deposit
{
    public class DepositLL
    {
        DepositDL _dacDepositDL = new DepositDL();
        internal List<tt_dep_sub_cash_book> PopulateSubCashBookDeposit(p_report_param prp)
        {
            return _dacDepositDL.PopulateSubCashBookDeposit(prp);

        }

        internal List<tt_sbca_dtl_list> PopulateDLSavings(p_report_param prp)
        {
            return _dacDepositDL.PopulateDLSavings(prp);
        }

        internal List<tt_sbca_dtl_list> PopulateDLRecuring(p_report_param prp)
        {
            return _dacDepositDL.PopulateDLRecuring(prp);
        }

        internal List<tt_sbca_dtl_list> PopulateDLFixedDeposit(p_report_param prp)
        {
            return _dacDepositDL.PopulateDLFixedDeposit(prp);
        }

        internal List<tt_acc_stmt> PopulateASSaving(p_report_param prp)
        {
            return _dacDepositDL.PopulateASSaving(prp);
        }

        internal List<tm_deposit> PopulateASRecuring(p_report_param prp)
        {
            return _dacDepositDL.PopulateASRecuring(prp);
        }

        internal List<tm_deposit> PopulateASFixedDeposit(p_report_param prp)
        {
            return _dacDepositDL.PopulateASFixedDeposit(prp);
        }
        internal List<tt_opn_cls_register> PopulateOpenCloseRegister(p_report_param prp)
        {
            return _dacDepositDL.PopulateOpenCloseRegister(prp);
        }

        internal List<tm_deposit> PopulateNearMatDetails(p_report_param prp)
        {
            return _dacDepositDL.PopulateNearMatDetails(prp);
        }
        internal List<tm_deposit> GetDepositWithChild(tm_deposit dep)
        {
            return _dacDepositDL.GetDepositWithChild(dep);
        }
        internal List<td_outward_payment> GetNeftOutDtls(p_report_param prp)
        {
            return _dacDepositDL.GetNeftOutDtls(prp);
        }

        internal List<neft_inward> NeftInward(p_report_param prp)
        {
            return _dacDepositDL.NeftInward(prp);
        }

        internal List<passbook_print> PassBookPrint(p_report_param prp)
        {
            decimal curr_bal;
            List<passbook_print> passbook = _dacDepositDL.PassBookPrint(prp);

            if (passbook != null && passbook.Count > 0)
            {
                curr_bal = passbook[0].balance_amt;
                for (int i = passbook.Count - 1; i >= 0; i--)
                {
                    passbook[i].balance_amt = curr_bal;

                    if (passbook[i].trans_type.ToUpper() == "D")
                    {
                        curr_bal = curr_bal - passbook[i].amount;
                        passbook[i].withdrawal = 0;
                        passbook[i].deposit = passbook[i].amount;
                    }
                    else
                    {
                        curr_bal = curr_bal + passbook[i].amount;
                        passbook[i].withdrawal = passbook[i].amount;
                        passbook[i].deposit = 0;
                    }
                }
            }

            return passbook;
        }

        internal List<active_standing_instr> GetActiveStandingInstr(p_report_param prp)
        {
            return _dacDepositDL.GetActiveStandingInstr(prp);
        }

        internal List<standing_instr_exe> GetStandingInstrExe(p_report_param prp)
        {
            return _dacDepositDL.GetStandingInstrExe(prp);
        }

    }
}
