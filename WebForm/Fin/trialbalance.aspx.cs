using Microsoft.Reporting.WebForms;
using RDLCReportServer.Model;
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
    public partial class trialbalance : System.Web.UI.Page
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
                    RV_trialbalance.LocalReport.ReportPath = Server.MapPath("~/Reports/Fin/trialbalance.rdlc");
                    RV_trialbalance.LocalReport.DataSources.Clear();
                    RV_trialbalance.KeepSessionAlive = false;
                    RV_trialbalance.AsyncRendering = false;
                    var prp = new p_report_param();
                    prp.brn_cd = Request.QueryString["brn_cd"];
                    string brn_name = _masterLL.GetBranchMaster(prp.brn_cd);
                    prp.trial_dt = Convert.ToDateTime(Request.QueryString["trial_dt"]);
                    prp.pl_acc_cd = Convert.ToInt32(Request.QueryString["pl_acc_cd"]);
                    prp.gp_acc_cd = Convert.ToInt32(Request.QueryString["gp_acc_cd"]);
                    List<tt_trial_balance> TrialBalance = _FinanceReportLL.PopulateTrialBalance(prp);
                    if (TrialBalance.Any())
                    {
                        foreach (var x in TrialBalance)
                    {
                        if (x.acc_type =="1")
                           x.acc_type_desc = "Liability";
                        else if (x.acc_type == "2")
                            x.acc_type_desc = "Asset";
                        else if (x.acc_type == "3")
                            x.acc_type_desc = "Purchase";
                        else
                            x.acc_type_desc = "Sale";
                    }
                    dataSet = Extension.ToDataSet(TrialBalance);
                    ReportDataSource rdc = new ReportDataSource("trialbalance", dataSet.Tables[0]);
                    ReportParameter[] paramss = new ReportParameter[3];
                    paramss[0] = new ReportParameter("p_bank_name", BC.bank_desc, false);
                    paramss[1] = new ReportParameter("p_branch_name", brn_name, false);
                    paramss[2] = new ReportParameter("p_from_dt", Request.QueryString["trial_dt"], false);
                    RV_trialbalance.LocalReport.SetParameters(paramss);
                    RV_trialbalance.LocalReport.DataSources.Add(rdc);
                    RV_trialbalance.LocalReport.Refresh();



                }
                    else
                {
                        RV_trialbalance.Visible = false;
                    NoDataFound.Visible = true;
                }

            }
                catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                    RV_trialbalance.Visible = false;
                NoDataFound.Visible = true;
            }
        }

        }
    }
}
