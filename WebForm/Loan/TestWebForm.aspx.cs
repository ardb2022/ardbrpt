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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RDLCReportServer.WebForm.Loan
{
    public partial class TestWebForm : System.Web.UI.Page
    {

        private DataSet dataSet;

        public object RV_ASF { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    LoanLL _LoanLL = new LoanLL();


                    BankConfigMstLL _masterLL = new BankConfigMstLL();
                    BankConfig BC = OrclDbConnection.getBankConfigFromDB();

                    //ReportViewerTest.LocalReport.ReportPath = Server.MapPath("~/Reports/Deposit/asfixed.rdlc");
                    //ReportViewerTest.LocalReport.DataSources.Clear();
                    //ReportViewerTest.KeepSessionAlive = true;
                    //ReportViewerTest.AsyncRendering = true;

                    var prp = new p_report_param();
                    //prp.from_dt = Convert.ToDateTime(Request.QueryString["from_dt"]);
                    //prp.to_dt = Convert.ToDateTime(Request.QueryString["to_dt"]);
                    //prp.brn_cd = Request.QueryString["brn_cd"];
                    //prp.acc_type_cd = Convert.ToInt16(Request.QueryString["acc_type_cd"]);
                    //prp.acc_num = Request.QueryString["acc_num"];
                    //prp.const_cd = Convert.ToInt16(Request.QueryString["renew_id"]);
                    //string brn_name = _masterLL.GetBranchMaster(prp.brn_cd);


                    //Test 1 Ok http://localhost:63011/WebForm/Loan/TestWebForm?brn_cd=101&acc_cd=23103&adt_dt=01/01/2021    
                    //prp.brn_cd = Request.QueryString["brn_cd"];
                    //prp.acc_cd = Convert.ToInt32(Request.QueryString["acc_cd"]);
                    //prp.adt_dt = Convert.ToDateTime(Request.QueryString["adt_dt"]);
                    // List<tt_detailed_list_loan> depositdetails = _LoanLL.PopulateLoanDetailedList(prp);  Test Ok

                    //Test 2 OK http://localhost:63011/WebForm/Loan/TestWebForm?brn_cd=101&from_dt=01/01/2018&to_dt=01/01/2019
                    //prp.from_dt = Convert.ToDateTime(Request.QueryString["from_dt"]);
                    //prp.to_dt = Convert.ToDateTime(Request.QueryString["to_dt"]);
                    //prp.brn_cd = Request.QueryString["brn_cd"];
                    //List<tm_loan_all> tm_loan = _LoanLL.PopulateLoanDisburseReg(prp);


                    //// Test 3 OK http://localhost:63011/WebForm/Loan/TestWebForm?brn_cd=101&loan_id=1013645&from_dt=01/01/2018&to_dt=01/01/2019
                    //prp.from_dt = Convert.ToDateTime(Request.QueryString["from_dt"]);
                    //prp.to_dt = Convert.ToDateTime(Request.QueryString["to_dt"]);
                    //prp.brn_cd = Request.QueryString["brn_cd"];
                    //prp.loan_id = Request.QueryString["loan_id"];
                    //List<gm_loan_trans> gm_loan = _LoanLL.PopulateLoanStatement(prp);

                    // Test 4 OK http://localhost:63011/WebForm/Loan/TestWebForm?brn_cd=101&from_dt=01/01/2018&to_dt=01/01/2019
                    //prp.from_dt = Convert.ToDateTime(Request.QueryString["from_dt"]);
                    //prp.to_dt = Convert.ToDateTime(Request.QueryString["to_dt"]);
                    //prp.brn_cd = Request.QueryString["brn_cd"];
                    //List<gm_loan_trans> gm_loan = _LoanLL.PopulateRecoveryRegister(prp);

                    // Test 5 OK http://localhost:63011/WebForm/Loan/TestWebForm?brn_cd=101&adt_as_on_dt=01/01/2021
                    prp.brn_cd = Request.QueryString["brn_cd"];
                    prp.adt_as_on_dt = Convert.ToDateTime(Request.QueryString["adt_as_on_dt"]);
                    List<tt_loan_sub_cash_book> cash_book = _LoanLL.PopulateLoanSubCashBook(prp);


                    //var dep = new tm_deposit();
                    //dep.brn_cd = Request.QueryString["brn_cd"];
                    //dep.acc_type_cd = Convert.ToInt16(Request.QueryString["acc_type_cd"]);
                    //dep.acc_num = Request.QueryString["acc_num"];
                    //List<tm_deposit> customerdetails = _DepositLL.GetDepositWithChild(dep);

                    //dataSet = Extension.ToDataSet(depositdetails); // Model to Data set conversion -----
                    //ReportDataSource rdc = new ReportDataSource("asfixed", dataSet.Tables[0]); // System data set to My defined data set

                    //ReportParameter[] paramss = new ReportParameter[7];
                    //paramss[0] = new ReportParameter("p_bank_name", BC.bankname, false);

                    //paramss[1] = new ReportParameter("p_branch_name", brn_name, false);
                    //paramss[2] = new ReportParameter("p_from_dt", Request.QueryString["from_dt"], false);
                    //paramss[3] = new ReportParameter("p_acc_num", customerdetails[0].acc_num, false);
                    //paramss[4] = new ReportParameter("p_name", customerdetails[0].cust_name, false);
                    //paramss[5] = new ReportParameter("p_bal", Convert.ToString(depositdetails[0].prn_amt), false);
                    //paramss[6] = new ReportParameter("p_open_dt", Convert.ToString(depositdetails[0].opening_dt), false);
                    //RV_ASF.LocalReport.SetParameters(paramss);
                    //RV_ASF.LocalReport.DataSources.Add(rdc);
                    //RV_ASF.LocalReport.Refresh();


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}