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

namespace RDLCReportServer.WebForm
{
    public partial class cashaccount : System.Web.UI.Page
    {
        private DataSet dataSet;
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
                    RVCashAccount.LocalReport.ReportPath = Server.MapPath("~/Reports/Fin/cashaccount.rdlc");
                    RVCashAccount.LocalReport.DataSources.Clear();
                    RVCashAccount.KeepSessionAlive = false;
                    RVCashAccount.AsyncRendering = false;
                    var prp = new p_report_param();
                      prp.brn_cd = Request.QueryString["brn_cd"];
                    string brn_name = _masterLL.GetBranchMaster(prp.brn_cd);
                    prp.from_dt = Convert.ToDateTime(Request.QueryString["from_dt"]);
                      prp.to_dt = Convert.ToDateTime(Request.QueryString["to_dt"]);
                      prp.acc_cd = Convert.ToInt32(Request.QueryString["acc_cd"]);
                    List<tt_cash_account> DailyCashBook = _FinanceReportLL.PopulateDailyCashAccount(prp);
                    if (DailyCashBook.Any())
                    {
                        dataSet = Extension.ToDataSet(DailyCashBook);
                    ReportDataSource rdc = new ReportDataSource("cashaccount", dataSet.Tables[0]);
                    ReportParameter[] paramss = new ReportParameter[4];
                    paramss[0] = new ReportParameter("p_bank_name", BC.bank_desc, false);
                    paramss[1] = new ReportParameter("p_branch_name", brn_name, false);
                    paramss[2] = new ReportParameter("p_from_dt", Request.QueryString["from_dt"], false);
                    paramss[3] = new ReportParameter("p_to_dt", Request.QueryString["to_dt"], false);
                    RVCashAccount.LocalReport.SetParameters(paramss);
                    RVCashAccount.LocalReport.DataSources.Add(rdc);
                    RVCashAccount.LocalReport.Refresh();



                }
                    else
                {
                        RVCashAccount.Visible = false;
                    NoDataFound.Visible = true;
                }

            }
                catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                    RVCashAccount.Visible = false;
                NoDataFound.Visible = true;
            }
        }
        }
    }
}