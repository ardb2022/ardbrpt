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
    public partial class loandisbursement : System.Web.UI.Page
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


                    //  http://localhost:63011/WebForm/Loan/loandisbursement?brn_cd=101&from_dt=01/01/2018&to_dt=01/01/2019 

                    var prp = new p_report_param();
                    prp.from_dt = Convert.ToDateTime(Request.QueryString["from_dt"]);
                    prp.to_dt = Convert.ToDateTime(Request.QueryString["to_dt"]);
                    prp.brn_cd = Request.QueryString["brn_cd"];
                    List<tm_loan_all> loanDisburseRegList = _LoanLL.PopulateLoanDisburseReg(prp);
                    if (loanDisburseRegList.Any())
                    {
                        RVLoanDisburse.LocalReport.ReportPath = Server.MapPath("~/Reports/Loan/loandisbursement.rdlc");
                    RVLoanDisburse.LocalReport.DataSources.Clear();
                    RVLoanDisburse.KeepSessionAlive = true;
                    RVLoanDisburse.AsyncRendering = true;

                    foreach (var x in loanDisburseRegList)
                    {
                        var filtCat = category.FirstOrDefault(y => y.acc_type_cd == x.acc_cd);
                        if (filtCat != null && filtCat.acc_type_cd > 0)
                        {
                            x.acc_typ_dsc = filtCat.acc_type_desc;
                        }
                        else
                        {
                            x.acc_typ_dsc = "NA";
                        }                        
                    }

                    dataSet = Extension.ToDataSet(loanDisburseRegList);
                    ReportDataSource rdc = new ReportDataSource("LoanDisburseReg", dataSet.Tables[0]);

                    string brn_name = _masterLL.GetBranchMaster(prp.brn_cd);

                    ReportParameter[] paramss = new ReportParameter[4];
                    paramss[0] = new ReportParameter("p_bank_name", BC.bank_desc, false);
                    paramss[1] = new ReportParameter("p_branch_name", brn_name, false);
                    paramss[2] = new ReportParameter("p_from_dt", prp.from_dt.ToString(), false);
                    paramss[3] = new ReportParameter("p_to_dt", prp.to_dt.ToString(), false);

                    RVLoanDisburse.LocalReport.SetParameters(paramss);

                    RVLoanDisburse.LocalReport.DataSources.Add(rdc);
                    RVLoanDisburse.LocalReport.Refresh();
                }
                    else
                {
                        RVLoanDisburse.Visible = false;
                    NoDataFound.Visible = true;
                }

            }
                catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                    RVLoanDisburse.Visible = false;
                NoDataFound.Visible = true;
            }
        }

        }
    }
}
