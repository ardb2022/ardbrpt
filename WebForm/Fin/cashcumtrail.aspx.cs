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
    public partial class cashcumtrail : System.Web.UI.Page
    {
        private DataSet dataSet;
        public void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    NoDataFound.Visible = false;
                    FinanceReportLL _FinanceReportLL = new FinanceReportLL();
                    BankConfigMstLL _masterLL = new BankConfigMstLL();
                    BankConfig BC = OrclDbConnection.getBankConfigFromDB();
                    RV_cashcumtrail.LocalReport.ReportPath = Server.MapPath("~/Reports/Fin/cashcumtrail.rdlc");
                    RV_cashcumtrail.LocalReport.DataSources.Clear();
                    RV_cashcumtrail.KeepSessionAlive = false;
                    RV_cashcumtrail.AsyncRendering = false;
                    var prp = new p_report_param();
                    prp.brn_cd = Request.QueryString["brn_cd"];
                    prp.from_dt = Convert.ToDateTime(Request.QueryString["from_dt"]);
                    prp.to_dt = Convert.ToDateTime(Request.QueryString["to_dt"]);
                    
                    List<tt_cash_cum_trial> CashCumTrail = _FinanceReportLL.PopulateCashCumTrial(prp);
                    if (CashCumTrail.Any())
                    {
                        string brn_name = _masterLL.GetBranchMaster(prp.brn_cd);
                        dataSet = Extension.ToDataSet(CashCumTrail);
                    ReportDataSource rdc = new ReportDataSource("cashcumtrail", dataSet.Tables[0]);
                    ReportParameter[] paramss = new ReportParameter[4];
                    paramss[0] = new ReportParameter("p_bank_name", BC.bank_desc, false);
                    paramss[1] = new ReportParameter("p_branch_name", brn_name, false);
                    paramss[2] = new ReportParameter("p_from_dt", Request.QueryString["from_dt"], false);
                    paramss[3] = new ReportParameter("p_to_dt", Request.QueryString["to_dt"], false);
                    RV_cashcumtrail.LocalReport.SetParameters(paramss);
                    RV_cashcumtrail.LocalReport.DataSources.Add(rdc);
                    RV_cashcumtrail.LocalReport.Refresh();



                }
                    else
                {
                        RV_cashcumtrail.Visible = false;
                    NoDataFound.Visible = true;
                }

            }
                catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                    RV_cashcumtrail.Visible = false;
                NoDataFound.Visible = true;
            }
        }

        }
    }
}