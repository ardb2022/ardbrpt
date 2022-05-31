
using System;
using System.Collections.Generic;
using SBWSFinanceApi.DL;
using SBWSFinanceApi.Models;

namespace SBWSFinanceApi.LL
{
    public class FinanceReportLL
    {
        CashCumTrialDL _dacCashCumTrialDL = new CashCumTrialDL();
        internal List<tt_cash_cum_trial> PopulateCashCumTrial(p_report_param prp)
        {
            return _dacCashCumTrialDL.PopulateCashCumTrial(prp);
        }

        TrialBalanceDL _dacTrialBalanceDL = new TrialBalanceDL();
        internal List<tt_trial_balance> PopulateTrialBalance(p_report_param prp)
        {
            return _dacTrialBalanceDL.PopulateTrialBalance(prp);
        }

        DailyCashBookDL _dacDailyCashBookDL = new DailyCashBookDL();
        internal List<tt_cash_account> PopulateDailyCashBook(p_report_param prp)
        {
            return _dacDailyCashBookDL.PopulateDailyCashBook(prp);
        }
        internal List<tt_cash_account> PopulateDailyCashAccount(p_report_param prp)
        {
            return _dacDailyCashBookDL.PopulateDailyCashAccount(prp);
        }

        DayScrollBookDL _dacDayScrollBookDL = new DayScrollBookDL();
        internal List<tt_day_scroll> PopulateDayScrollBook(p_report_param prp)
        {
            return _dacDayScrollBookDL.PopulateDayScrollBook(prp);
        }
        internal List<tt_gl_trans> getGeneralLedgerTransactionDtls(p_report_param prm, bool gtdetails = false)
        {
            var _dac = new RptGeneralLedgerTransactionDtlsDL();
            return _dac.getGeneralLedgerTransactionDtls(prm, gtdetails);
        }
        internal List<tt_gl_trans> getGeneralLedgerTransactionDtlsOrdrByVuchrID(p_report_param prm, bool gtdetails = false)
        {
            var _dac = new RptGeneralLedgerTransactionDtlsDL();
            return _dac.getGeneralLedgerTransactionDtls(prm, true);
        }
        BalanceSheet _dacBalanceSheetDL = new BalanceSheet();
        internal List<tt_balance_sheet> PopulateBalanceSheet(p_report_param prp)
        {
            return _dacBalanceSheetDL.PopulateBalanceSheet(prp);
        }
        ProfitandLoss _dacpl = new ProfitandLoss();
        internal List<tt_pl_book> PopulateProfitandLoss(p_report_param prp)
        {
            return _dacpl.PopulateProfitandLoss(prp);
        }
        TradingAc _datrdac = new TradingAc();
        internal List<tt_trading_account> PopulateTradingAc(p_report_param prp)
        {
            return _datrdac.PopulateTradingAc(prp);
        }
    }
}