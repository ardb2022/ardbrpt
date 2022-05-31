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
    public partial class depositsubcashbook : System.Web.UI.Page
    {
        private DataSet dataSet;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                try
                {
                    //http://localhost:63011/WebForm/Deposit/depositsubcashbook?brn_cd=101&from_dt=01/01/2021 
                    NoDataFound.Visible = false;
                    DepositLL _DepositLL = new DepositLL();
                    BankConfigMstLL _masterLL = new BankConfigMstLL();
                    BankConfig BC = OrclDbConnection.getBankConfigFromDB();
                    RV_DcashBook.LocalReport.ReportPath = Server.MapPath("~/Reports/Deposit/depositsubcashbook.rdlc");
                    RV_DcashBook.LocalReport.DataSources.Clear();
                    RV_DcashBook.KeepSessionAlive = true;
                    RV_DcashBook.AsyncRendering = true;
                    var prp = new p_report_param();
                    prp.from_dt = Convert.ToDateTime(Request.QueryString["from_dt"]);
                    prp.brn_cd = Request.QueryString["brn_cd"];
                    string brn_name = _masterLL.GetBranchMaster(prp.brn_cd);
                    List<tt_dep_sub_cash_book> depositdetails = _DepositLL.PopulateSubCashBookDeposit(prp);
                    if (depositdetails.Any())
                    {

                    List<mm_acc_type> category = _masterLL.GetAccountTypeMaster();
                    List<mm_constitution> constitution = _masterLL.GetConstitution();
                    foreach (var x in depositdetails)
                    {
                        var filtCat = category.FirstOrDefault(y => y.acc_type_cd == x.acc_type_cd);
                        if (filtCat != null && filtCat.acc_type_cd > 0)
                        {
                            x.acc_type_desc = filtCat.acc_type_desc;
                        }
                        else
                        {
                            x.acc_type_desc = "NA";
                        }
                        var filtCon = constitution.FirstOrDefault(y => y.constitution_cd == x.constitution_cd);
                        if (filtCon != null && filtCon.constitution_cd > 0)
                        {
                            x.constitution_desc = filtCon.constitution_desc;
                        }
                        else
                        {
                            x.constitution_desc = "NA";
                        }

                    }
                    dataSet = Extension.ToDataSet(depositdetails);
                    ReportDataSource rdc = new ReportDataSource("depositsubcashbook", dataSet.Tables[0]);
                    ReportParameter[] paramss = new ReportParameter[3];
                    paramss[0] = new ReportParameter("p_bank_name", BC.bank_name, false);
                    paramss[1] = new ReportParameter("p_branch_name", brn_name, false);
                    paramss[2] = new ReportParameter("p_from_dt", Request.QueryString["from_dt"], false);
                    RV_DcashBook.LocalReport.SetParameters(paramss);
                    RV_DcashBook.LocalReport.DataSources.Add(rdc);
                    RV_DcashBook.LocalReport.Refresh();
                }
                    else
                {
                    RV_DcashBook.Visible = false;
                    NoDataFound.Visible = true;
                }

            }
                catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                RV_DcashBook.Visible = false;
                NoDataFound.Visible = true;
            }
        }


        }
    }
}
