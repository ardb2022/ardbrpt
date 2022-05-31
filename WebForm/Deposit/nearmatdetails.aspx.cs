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
    public partial class nearmatdetails : System.Web.UI.Page
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
                    RV_NM.LocalReport.ReportPath = Server.MapPath("~/Reports/Deposit/nearmatdetails.rdlc");
                    RV_NM.LocalReport.DataSources.Clear();
                    RV_NM.KeepSessionAlive = true;
                    RV_NM.AsyncRendering = true;
                    var prp = new p_report_param();
                    prp.from_dt = Convert.ToDateTime(Request.QueryString["from_dt"]);
                    prp.to_dt = Convert.ToDateTime(Request.QueryString["to_dt"]);
                    prp.brn_cd = Request.QueryString["brn_cd"];
                    List<tm_deposit> depositdetails = _DepositLL.PopulateNearMatDetails(prp);
                    if (depositdetails.Any())
                    {
                        string brn_name = _masterLL.GetBranchMaster(prp.brn_cd);
                        List<mm_acc_type> category = _masterLL.GetAccountTypeMaster();
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
                       

                    }
                    dataSet = Extension.ToDataSet(depositdetails);
                    ReportDataSource rdc = new ReportDataSource("nearmatdetails", dataSet.Tables[0]);
                    ReportParameter[] paramss = new ReportParameter[3];
                    paramss[0] = new ReportParameter("p_bank_name", BC.bank_desc, false);
                    paramss[1] = new ReportParameter("p_branch_name", brn_name, false);
                    paramss[2] = new ReportParameter("p_from_dt", Request.QueryString["from_dt"], false);
                    RV_NM.LocalReport.SetParameters(paramss);
                    RV_NM.LocalReport.DataSources.Add(rdc);
                    RV_NM.LocalReport.Refresh();


                }
                    else
                {
                        RV_NM.Visible = false;
                    NoDataFound.Visible = true;
                }

            }
                catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                    RV_NM.Visible = false;
                NoDataFound.Visible = true;
            }
        }

        }
    }
}