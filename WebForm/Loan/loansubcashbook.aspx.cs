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
    public partial class loansubcashbook : System.Web.UI.Page
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
                    List<mm_acc_type> category = _masterLL.GetAccountTypeMaster();

                    BankConfig BC = OrclDbConnection.getBankConfigFromDB();

                    // http://localhost:63011/WebForm/Loan/loansubcashbook?brn_cd=101&adt_as_on_dt=01/01/2021
                    var prp = new p_report_param();
                    prp.brn_cd = Request.QueryString["brn_cd"];
                    prp.adt_as_on_dt = Convert.ToDateTime(Request.QueryString["adt_as_on_dt"]);
                    List<tt_loan_sub_cash_book> loanSubCashBook = _LoanLL.PopulateLoanSubCashBook(prp);
                    if (loanSubCashBook.Any())
                    {

                        RVLoanSubCashBook.LocalReport.ReportPath = Server.MapPath("~/Reports/Loan/loansubcashbook.rdlc");
                    RVLoanSubCashBook.LocalReport.DataSources.Clear();
                    RVLoanSubCashBook.KeepSessionAlive = true;
                    RVLoanSubCashBook.AsyncRendering = true;

                    foreach (var x in loanSubCashBook)
                    {
                        var filtCat = category.FirstOrDefault(y => y.acc_type_cd == x.acc_type_cd);
                        if (filtCat != null && filtCat.acc_type_cd > 0)
                        {
                            x.acc_typ_dsc = filtCat.acc_type_desc;
                        }
                        else
                        {
                            x.acc_typ_dsc = "NA";
                        }
                    }

                    dataSet = Extension.ToDataSet(loanSubCashBook);
                    ReportDataSource rdc = new ReportDataSource("SubCashBook", dataSet.Tables[0]);

                    string brn_name = _masterLL.GetBranchMaster(prp.brn_cd);

                    ReportParameter[] paramss = new ReportParameter[3];
                    paramss[0] = new ReportParameter("p_bank_name", BC.bank_name, false);
                    paramss[1] = new ReportParameter("p_branch_name", brn_name, false);
                    paramss[2] = new ReportParameter("p_to_dt", prp.adt_as_on_dt.ToString(), false);

                    RVLoanSubCashBook.LocalReport.SetParameters(paramss);

                    RVLoanSubCashBook.LocalReport.DataSources.Add(rdc);
                    RVLoanSubCashBook.LocalReport.Refresh();
                }
                    else
                {
                        RVLoanSubCashBook.Visible = false;
                    NoDataFound.Visible = true;
                }

            }
                catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                    RVLoanSubCashBook.Visible = false;
                NoDataFound.Visible = true;
            }
        }

        }
    }
}
