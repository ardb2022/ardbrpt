using Microsoft.Reporting.WebForms;
using RDLCReportServer.Util;
using SBWSFinanceApi.LL;
using SBWSFinanceApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RDLCReportServer.WebForm.Fin
{
    public partial class tradingac : System.Web.UI.Page
    {
        private DataSet dataSetasset;
        private DataSet dataSetliability;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                try
                {
                    NoDataFound.Visible = false;
                    FinanceReportLL _FinanceReportLL = new FinanceReportLL();
                    BankConfigMstLL _masterLL = new BankConfigMstLL();
                    BankConfig BC = OrclDbConnection.getBankConfigFromDB();
                    RV_TA.LocalReport.ReportPath = Server.MapPath("~/Reports/Fin/tradingac.rdlc");
                    RV_TA.LocalReport.DataSources.Clear();
                    RV_TA.KeepSessionAlive = false;
                    RV_TA.AsyncRendering = false;
                    var prp = new p_report_param();
                    prp.brn_cd = Request.QueryString["brn_cd"];
                    string brn_name = _masterLL.GetBranchMaster(prp.brn_cd);
                    prp.from_dt = Convert.ToDateTime(Request.QueryString["from_dt"]);
                    prp.to_dt = Convert.ToDateTime(Request.QueryString["to_dt"]);
                    List<tt_trading_account> DailyCashBook = _FinanceReportLL.PopulateTradingAc(prp);
                    if (DailyCashBook.Any())
                    {
                        List<tt_trading_account> DailyCashBookd = DailyCashBook.Where(x => x.type == "Debit").ToList();
                    dataSetasset = Extension.ToDataSet(DailyCashBookd);
                    ReportDataSource rdca = new ReportDataSource("tradingacdr", dataSetasset.Tables[0]);
                    List<tt_trading_account> DailyCashBookc = DailyCashBook.Where(x => x.type == "Credit").ToList();
                    dataSetliability = Extension.ToDataSet(DailyCashBookc);
                    ReportDataSource rdcl = new ReportDataSource("tradingaccr", dataSetliability.Tables[0]);
                    ReportParameter[] paramss = new ReportParameter[4];
                    paramss[0] = new ReportParameter("p_bank_name", BC.bank_desc, false);
                    paramss[1] = new ReportParameter("p_branch_name", brn_name, false);
                    paramss[2] = new ReportParameter("p_from_dt", Request.QueryString["from_dt"], false);
                    paramss[3] = new ReportParameter("p_to_dt", Request.QueryString["to_dt"], false);
                    RV_TA.LocalReport.SetParameters(paramss);
                    RV_TA.LocalReport.DataSources.Add(rdca);
                    RV_TA.LocalReport.Refresh();
                    RV_TA.LocalReport.DataSources.Add(rdcl);
                    RV_TA.LocalReport.Refresh();

                }
                    else
                {
                        RV_TA.Visible = false;
                    NoDataFound.Visible = true;
                }

            }
                catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                    RV_TA.Visible = false;
                NoDataFound.Visible = true;
            }
        }
        }
    }
}