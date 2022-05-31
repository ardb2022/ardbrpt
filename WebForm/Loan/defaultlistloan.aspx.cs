using Microsoft.Reporting.WebForms;
using RDLCReportServer.LL.Loan;
using RDLCReportServer.Model;
using RDLCReportServer.Util;
using SBWSFinanceApi.LL;
using SBWSFinanceApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;

namespace RDLCReportServer.WebForm.Loan
{
    public partial class defaultlistloan : System.Web.UI.Page
    {
        private DataSet dataSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    // http://localhost:63011/WebForm/Loan/defaultlistloan?brn_cd=101&acc_cd=23103&adt_dt=01/01/2022 
                    NoDataFound.Visible = false;
                    LoanLL _LoanLL = new LoanLL();
                    BankConfigMstLL _masterLL = new BankConfigMstLL();
                    List<mm_acc_type> category = _masterLL.GetAccountTypeMaster();

                    BankConfigMst BC = new BankConfigMstLL().ReadAllConfiguration();

                    var prp = new p_report_param();
                    prp.brn_cd = Request.QueryString["brn_cd"];
                    prp.acc_cd = Convert.ToInt32(Request.QueryString["acc_cd"]);
                    prp.adt_dt = Convert.ToDateTime(Request.QueryString["adt_dt"]);

                    List<tt_detailed_list_loan> loanDefaultList = _LoanLL.GetDefaultList(prp);
                    if (loanDefaultList.Any())
                    {
                        RV_DefaultList.LocalReport.ReportPath = Server.MapPath("~/Reports/Loan/loandefaultlist.rdlc");
                    RV_DefaultList.LocalReport.DataSources.Clear();
                    RV_DefaultList.KeepSessionAlive = true;
                    RV_DefaultList.AsyncRendering = true;

                    dataSet = Extension.ToDataSet(loanDefaultList);
                    ReportDataSource rdc = new ReportDataSource("LoanDefaultList", dataSet.Tables[0]);

                    string brn_name = _masterLL.GetBranchMaster(prp.brn_cd);
                    ReportParameter[] paramss = new ReportParameter[6];

                    paramss[0] = new ReportParameter("p_bank_name", BC.bankname, false);
                    paramss[1] = new ReportParameter("p_branch_name", brn_name, false);
                    paramss[2] = new ReportParameter("p_to_dt", prp.adt_dt.ToShortDateString(), false);
                    if (loanDefaultList != null && loanDefaultList.Count > 0)
                    {
                        paramss[3] = new ReportParameter("acc_cd", loanDefaultList[0].acc_cd.ToString(), false);
                        paramss[4] = new ReportParameter("acc_name", loanDefaultList[0].acc_name.ToString(), false);
                    }
                    else
                    {
                        paramss[3] = null;
                        paramss[4] = null;
                    }
                    paramss[5] = new ReportParameter("p_branch_code", prp.brn_cd, false);

                    RV_DefaultList.LocalReport.SetParameters(paramss);
                    RV_DefaultList.LocalReport.DataSources.Add(rdc);
                    RV_DefaultList.LocalReport.Refresh();
                }
                    else
                {
                        RV_DefaultList.Visible = false;
                    NoDataFound.Visible = true;
                }

            }
                catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                    RV_DefaultList.Visible = false;
                NoDataFound.Visible = true;
            }
        }

        }
    }
}

