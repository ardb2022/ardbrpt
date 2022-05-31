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
    public partial class balancesheet : System.Web.UI.Page
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
                    BankConfigMst BC = new BankConfigMstLL().ReadAllConfiguration();
                    RV_BS.LocalReport.ReportPath = Server.MapPath("~/Reports/Fin/balancesheet.rdlc");
                    RV_BS.LocalReport.DataSources.Clear();
                    RV_BS.KeepSessionAlive = false;
                    RV_BS.AsyncRendering = false;
                    var prp = new p_report_param();
                    prp.brn_cd = Request.QueryString["brn_cd"];
                    string brn_name = _masterLL.GetBranchMaster(prp.brn_cd);
                    prp.from_dt = Convert.ToDateTime(Request.QueryString["from_dt"]);
                    var y = Request.QueryString["from_dt"].Substring(6, 4);
                    var yr = String.Concat(Request.QueryString["from_dt"].Substring(6, 4),"-",(Convert.ToInt32(Request.QueryString["from_dt"].Substring(6, 4))+1).ToString());
                    List<tt_balance_sheet> DailyCashBook = _FinanceReportLL.PopulateBalanceSheet(prp);
                    if (DailyCashBook.Any())
                    {
                        List<tt_balance_sheet> DailyCashBooka = DailyCashBook.Where(x => x.type == "Asset").ToList();
                    dataSetasset = Extension.ToDataSet(DailyCashBooka);
                    ReportDataSource rdca = new ReportDataSource("balancesheetasset", dataSetasset.Tables[0]);
                    List<tt_balance_sheet> DailyCashBookl = DailyCashBook.Where(x => x.type == "Liability").ToList();
                    dataSetliability = Extension.ToDataSet(DailyCashBookl);
                    ReportDataSource rdcl = new ReportDataSource("balancesheetliability", dataSetliability.Tables[0]);
                    ReportParameter[] paramss = new ReportParameter[4];
                    paramss[0] = new ReportParameter("p_bank_name", BC.bankname, false);
                    paramss[1] = new ReportParameter("p_branch_name", brn_name, false);
                    paramss[2] = new ReportParameter("p_from_dt", Request.QueryString["from_dt"], false);
                    paramss[3] = new ReportParameter("p_year", yr, false);
                    RV_BS.LocalReport.SetParameters(paramss);
                    RV_BS.LocalReport.DataSources.Add(rdca);
                    RV_BS.LocalReport.Refresh();
                    RV_BS.LocalReport.DataSources.Add(rdcl);
                    RV_BS.LocalReport.Refresh();

                }
                    else
                {
                        RV_BS.Visible = false;
                    NoDataFound.Visible = true;
                }

            }
                catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                    RV_BS.Visible = false;
                NoDataFound.Visible = true;
            }
        }
        }
    }
}