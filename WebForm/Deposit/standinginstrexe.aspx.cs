using Microsoft.Reporting.WebForms;
using RDLCReportServer.LL.Deposit;
using RDLCReportServer.LL.UCIC;
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

namespace RDLCReportServer.WebForm.Deposit
{
    public partial class standinginstrexe : System.Web.UI.Page
    {
        private DataSet dataSet;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                try
                {
                    //  http://localhost:63011/WebForm/Deposit/standinginstrexe?brn_cd=101&adt_dt=09/09/2019

                    NoDataFound.Visible = false;
                    DepositLL _DepositLL = new DepositLL();
                    BankConfigMstLL _masterLL = new BankConfigMstLL();
                    BankConfigMst BC = new BankConfigMstLL().ReadAllConfiguration();

                    RV_StandingInstrExe.LocalReport.ReportPath = Server.MapPath("~/Reports/Deposit/StandingInstrExe.rdlc");
                    RV_StandingInstrExe.LocalReport.DataSources.Clear();
                    RV_StandingInstrExe.KeepSessionAlive = true;
                    RV_StandingInstrExe.AsyncRendering = true;

                    var prp = new p_report_param();

                    prp.brn_cd = Request.QueryString["brn_cd"];
                    prp.adt_dt = Convert.ToDateTime(Request.QueryString["adt_dt"]);

                    string brn_name = _masterLL.GetBranchMaster(prp.brn_cd);

                    List<standing_instr_exe> standingInstrList = _DepositLL.GetStandingInstrExe(prp);
                    if (standingInstrList.Any())
                    {
                        dataSet = Extension.ToDataSet(standingInstrList);

                    ReportDataSource rdc = new ReportDataSource("StandingInstrExe", dataSet.Tables[0]);
                    ReportParameter[] paramss = new ReportParameter[4];
                    paramss[0] = new ReportParameter("p_bank_name", BC.bankname, false);
                    paramss[1] = new ReportParameter("p_branch_name", brn_name, false);
                    paramss[2] = new ReportParameter("p_exe_date", prp.adt_dt.ToShortDateString(), false);
                    paramss[3] = new ReportParameter("p_branch_code", prp.brn_cd, false);

                    RV_StandingInstrExe.LocalReport.SetParameters(paramss);
                    RV_StandingInstrExe.LocalReport.DataSources.Add(rdc);
                    RV_StandingInstrExe.LocalReport.Refresh();


                }
                    else
                {
                        RV_StandingInstrExe.Visible = false;
                    NoDataFound.Visible = true;
                }

            }
                catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                    RV_StandingInstrExe.Visible = false;
                NoDataFound.Visible = true;
            }
        }


        }
    }
}
