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
    public partial class loanstatement : System.Web.UI.Page
    {
        private DataSet dataSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    NoDataFound.Visible = false;
                    LoanLL _LoanLL = new LoanLL();
                    BankConfigMstLL _masterLL = new BankConfigMstLL();

                    BankConfig BC = OrclDbConnection.getBankConfigFromDB();


                    // http://localhost:63011/WebForm/Loan/loanstatement?brn_cd=101&loan_id=1013645&from_dt=01/01/2018&to_dt=01/01/2019
                    var prp = new p_report_param();
                    prp.from_dt = Convert.ToDateTime(Request.QueryString["from_dt"]);
                    prp.to_dt = Convert.ToDateTime(Request.QueryString["to_dt"]);
                    prp.brn_cd = Request.QueryString["brn_cd"];
                    prp.loan_id = Request.QueryString["loan_id"];
                    List<gm_loan_trans> gmLoan = _LoanLL.PopulateLoanStatement(prp);
                    if (gmLoan.Any())
                    {
                        RVLoanStatement.LocalReport.ReportPath = Server.MapPath("~/Reports/Loan/loanstatement.rdlc");
                    RVLoanStatement.LocalReport.DataSources.Clear();
                    RVLoanStatement.KeepSessionAlive = true;
                    RVLoanStatement.AsyncRendering = true;

                    dataSet = Extension.ToDataSet(gmLoan);
                    ReportDataSource rdc = new ReportDataSource("LoanStatement", dataSet.Tables[0]);

                    string brn_name = _masterLL.GetBranchMaster(prp.brn_cd);

                    ReportParameter[] paramss = new ReportParameter[6];
                    paramss[0] = new ReportParameter("p_bank_name", BC.bank_desc, false);
                    paramss[1] = new ReportParameter("p_branch_name", brn_name, false);
                    paramss[2] = new ReportParameter("p_from_dt", prp.from_dt.ToString(), false);
                    paramss[3] = new ReportParameter("p_to_dt", prp.to_dt.ToString(), false);
                    paramss[4] = new ReportParameter("p_cust_name", gmLoan[0].cust_name, false);
                    paramss[5] = new ReportParameter("p_loan_id", prp.loan_id, false);

                    RVLoanStatement.LocalReport.SetParameters(paramss);
                    RVLoanStatement.LocalReport.DataSources.Add(rdc);
                    RVLoanStatement.LocalReport.Refresh();
                }
                    else
                {
                        RVLoanStatement.Visible = false;
                    NoDataFound.Visible = true;
                }

            }
                catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                    RVLoanStatement.Visible = false;
                NoDataFound.Visible = true;
            }
        }

        }
    }
}

