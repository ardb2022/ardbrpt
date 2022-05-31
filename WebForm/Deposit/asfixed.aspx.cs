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
    public partial class asfixed : System.Web.UI.Page
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
                    RV_ASF.LocalReport.ReportPath = Server.MapPath("~/Reports/Deposit/asfixed.rdlc");
                    RV_ASF.LocalReport.DataSources.Clear();
                    RV_ASF.KeepSessionAlive = true;
                    RV_ASF.AsyncRendering = true;
                    var prp = new p_report_param();
                    prp.from_dt = Convert.ToDateTime(Request.QueryString["from_dt"]);
                    prp.to_dt = Convert.ToDateTime(Request.QueryString["to_dt"]);
                    prp.brn_cd = Request.QueryString["brn_cd"];
                    prp.acc_type_cd = Convert.ToInt16(Request.QueryString["acc_type_cd"]);
                    prp.acc_num = Request.QueryString["acc_num"];
                    prp.const_cd = Convert.ToInt16(Request.QueryString["renew_id"]);
                    string brn_name = _masterLL.GetBranchMaster(prp.brn_cd);
                    List<tm_deposit> depositdetails = _DepositLL.PopulateASFixedDeposit(prp);
                    if (depositdetails.Any())
                    {

                        var dep = new tm_deposit();
                        dep.brn_cd = Request.QueryString["brn_cd"];
                        dep.acc_type_cd = Convert.ToInt16(Request.QueryString["acc_type_cd"]);
                        dep.acc_num = Request.QueryString["acc_num"];
                        List<tm_deposit> customerdetails = _DepositLL.GetDepositWithChild(dep);
                        dataSet = Extension.ToDataSet(depositdetails);
                        ReportDataSource rdc = new ReportDataSource("asfixed", dataSet.Tables[0]);
                        ReportParameter[] paramss = new ReportParameter[7];
                        paramss[0] = new ReportParameter("p_bank_name", BC.bank_desc, false);
                        paramss[1] = new ReportParameter("p_branch_name", brn_name, false);
                        paramss[2] = new ReportParameter("p_from_dt", Request.QueryString["from_dt"], false);
                        paramss[3] = new ReportParameter("p_acc_num", customerdetails[0].acc_num, false);
                        paramss[4] = new ReportParameter("p_name", customerdetails[0].cust_name, false);
                        paramss[5] = new ReportParameter("p_bal", Convert.ToString(depositdetails[0].prn_amt), false);
                        paramss[6] = new ReportParameter("p_open_dt", Convert.ToString(depositdetails[0].opening_dt), false);
                        RV_ASF.LocalReport.SetParameters(paramss);
                        RV_ASF.LocalReport.DataSources.Add(rdc);
                        RV_ASF.LocalReport.Refresh();
                    }
                    else
                    {
                        RV_ASF.Visible = false;
                        NoDataFound.Visible = true;
                    }

                    }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    RV_ASF.Visible = false;
                    NoDataFound.Visible = true;
                }
            }

        }
    }
}