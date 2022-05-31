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
    public partial class detailedlistforloan : System.Web.UI.Page
    {
        private DataSet dataSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    // http://localhost:63011/WebForm/Loan/detailedlistforloan?brn_cd=101&acc_cd=23103&adt_dt=01/01/2021 
                    NoDataFound.Visible = false;
                    LoanLL _LoanLL = new LoanLL();
                    BankConfigMstLL _masterLL = new BankConfigMstLL();
                    List<mm_acc_type> category = _masterLL.GetAccountTypeMaster();

                    BankConfig BC = OrclDbConnection.getBankConfigFromDB();

                    var prp = new p_report_param();
                    prp.brn_cd = Request.QueryString["brn_cd"];
                    prp.acc_cd = Convert.ToInt32(Request.QueryString["acc_cd"]);
                    prp.adt_dt = Convert.ToDateTime(Request.QueryString["adt_dt"]);

                    List<tt_detailed_list_loan> loanDetailedList = _LoanLL.PopulateLoanDetailedList(prp);
                    if (loanDetailedList.Any())
                    {
                        RVDetailedListLoan.LocalReport.ReportPath = Server.MapPath("~/Reports/Loan/detailedlistforloan.rdlc");
                    RVDetailedListLoan.LocalReport.DataSources.Clear();
                    RVDetailedListLoan.KeepSessionAlive = true;
                    RVDetailedListLoan.AsyncRendering = true;

                    dataSet = Extension.ToDataSet(loanDetailedList);
                    ReportDataSource rdc = new ReportDataSource("LoanDetailedList", dataSet.Tables[0]);

                    

                    string brn_name = _masterLL.GetBranchMaster(prp.brn_cd);
                    ReportParameter[] paramss = new ReportParameter[5];
                    paramss[0] = new ReportParameter("p_bank_name", BC.bank_desc, false);
                    paramss[1] = new ReportParameter("p_branch_name", brn_name, false);
                    paramss[2] = new ReportParameter("p_to_dt", prp.adt_dt.ToString(), false);
                    if (loanDetailedList != null && loanDetailedList.Count > 0)
                    {
                        paramss[3] = new ReportParameter("acc_cd", loanDetailedList[0].acc_cd.ToString(), false);
                        paramss[4] = new ReportParameter("acc_name", loanDetailedList[0].acc_name.ToString(), false);
                    }
                    else
                    {
                        paramss[3] = null;
                        paramss[4] = null;
                    }

                    RVDetailedListLoan.LocalReport.SetParameters(paramss);

                    RVDetailedListLoan.LocalReport.DataSources.Add(rdc);
                    RVDetailedListLoan.LocalReport.Refresh();
                }
                    else
                {
                        RVDetailedListLoan.Visible = false;
                    NoDataFound.Visible = true;
                }

            }
                catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                    RVDetailedListLoan.Visible = false;
                NoDataFound.Visible = true;
            }
        }

        }
    }
}
