using Microsoft.Reporting.WebForms;
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

namespace RDLCReportServer.WebForm.Fin
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private DataSet dataSet;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                try
                {
                    NoDataFound.Visible = false;
                    FinanceReportLL _FinanceReportLL = new FinanceReportLL();
                    BankConfigMstLL _masterLL = new BankConfigMstLL();
                    BankConfigMst BC = new BankConfigMstLL().ReadAllConfiguration();
                    RV_PL.LocalReport.ReportPath = Server.MapPath("~/Reports/Fin/profitandloss.rdlc");
                    RV_PL.LocalReport.DataSources.Clear();
                    RV_PL.KeepSessionAlive = false;
                    RV_PL.AsyncRendering = false;
                    var prp = new p_report_param();
                    prp.brn_cd = Request.QueryString["brn_cd"];
                    string brn_name = _masterLL.GetBranchMaster(prp.brn_cd);
                    prp.from_dt = Convert.ToDateTime(Request.QueryString["from_dt"]);
                    List<tt_pl_book> DailyCashBook = _FinanceReportLL.PopulateProfitandLoss(prp);
                    if (DailyCashBook.Any())
                    {

                        List<m_acc_master> category = _masterLL.GetAccountMaster();
                    foreach (var x in DailyCashBook)
                    {
                        var filtCat = category.FirstOrDefault(y => y.acc_cd == x.cr_acc_cd);
                        if (filtCat != null && filtCat.acc_cd > 0)
                        {
                            x.cr_acc_desc = filtCat.acc_name;
                        }
                        else
                        {
                            x.cr_acc_desc = "";
                        }
                        var filtCat1 = category.FirstOrDefault(y => y.acc_cd == x.dr_acc_cd);
                        if (filtCat1 != null && filtCat1.acc_cd > 0)
                        {
                            x.dr_acc_desc = filtCat1.acc_name;
                        }
                        else
                        {
                            x.dr_acc_desc = "";
                        }
                    }
                    dataSet = Extension.ToDataSet(DailyCashBook);
                    ReportDataSource rdc = new ReportDataSource("profitandloss", dataSet.Tables[0]);
                    ReportParameter[] paramss = new ReportParameter[3];
                    paramss[0] = new ReportParameter("p_bank_name", BC.bankname, false);
                    paramss[1] = new ReportParameter("p_branch_name", brn_name, false);
                    paramss[2] = new ReportParameter("p_from_dt", Request.QueryString["from_dt"], false);
                    RV_PL.LocalReport.SetParameters(paramss);
                    RV_PL.LocalReport.DataSources.Add(rdc);
                    RV_PL.LocalReport.Refresh();

                }
                    else
                {
                        RV_PL.Visible = false;
                    NoDataFound.Visible = true;
                }

            }
                catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                    RV_PL.Visible = false;
                NoDataFound.Visible = true;
            }
        }
        }
    }
}