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
    public partial class asrecurring : System.Web.UI.Page
    {
        private DataSet dataSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    NoDataFound.Visible = false;
                    DepositLL _DepositLL = new DepositLL();
                    BankConfigMstLL _masterLL = new BankConfigMstLL();
                    BankConfig BC = OrclDbConnection.getBankConfigFromDB();
                    RV_ASR.LocalReport.ReportPath = Server.MapPath("~/Reports/Deposit/asrecurring.rdlc");
                    RV_ASR.LocalReport.DataSources.Clear();
                    RV_ASR.KeepSessionAlive = true;
                    RV_ASR.AsyncRendering = true;
                    var prp = new p_report_param();
                    prp.from_dt = Convert.ToDateTime(Request.QueryString["from_dt"]);
                    prp.to_dt = Convert.ToDateTime(Request.QueryString["to_dt"]);
                    prp.brn_cd = Request.QueryString["brn_cd"];
                    prp.acc_num = Request.QueryString["acc_num"];
                    string brn_name = _masterLL.GetBranchMaster(prp.brn_cd);
                    var dep = new tm_deposit();
                    dep.brn_cd = Request.QueryString["brn_cd"];
                    dep.acc_type_cd = Convert.ToInt16("6");
                    dep.acc_num = Request.QueryString["acc_num"];
                    List<tm_deposit> depositdetails = _DepositLL.PopulateASRecuring(prp);
                    if (depositdetails.Any())
                    {
                        List<tm_deposit> customerdetails = _DepositLL.GetDepositWithChild(dep);
                        dataSet = Extension.ToDataSet(depositdetails);
                        ReportDataSource rdc = new ReportDataSource("asrecurring", dataSet.Tables[0]);
                        ReportParameter[] paramss = new ReportParameter[6];
                        paramss[0] = new ReportParameter("p_bank_name", BC.bank_desc, false);
                        paramss[1] = new ReportParameter("p_branch_name", brn_name, false);
                        paramss[2] = new ReportParameter("p_from_dt", Request.QueryString["from_dt"], false);
                        paramss[3] = new ReportParameter("p_acc_num", customerdetails[0].acc_num, false);
                        paramss[4] = new ReportParameter("p_name", customerdetails[0].cust_name, false);
                        paramss[5] = new ReportParameter("p_bal", Convert.ToString(depositdetails[0].clr_bal), false);
                        RV_ASR.LocalReport.SetParameters(paramss);
                        RV_ASR.LocalReport.DataSources.Add(rdc);
                        RV_ASR.LocalReport.Refresh();
                    } else
                    {
                        RV_ASR.Visible = false;
                        NoDataFound.Visible = true;
                    }
                    

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    RV_ASR.Visible = false;
                    NoDataFound.Visible = true;
                }
            }

        }
    }
}