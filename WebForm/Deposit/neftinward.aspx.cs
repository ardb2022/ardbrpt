using Microsoft.Reporting.WebForms;
using RDLCReportServer.LL.Deposit;
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
    public partial class neftinward : System.Web.UI.Page
    {
        private DataSet dataSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {

                    //  http://localhost:63011/WebForm/Deposit/neftinward?brn_cd=101&from_dt=01/01/2018&to_dt=01/01/2020
                    NoDataFound.Visible = false;
                    DepositLL _DepositLL = new DepositLL();
                    BankConfigMstLL _masterLL = new BankConfigMstLL();
                    BankConfig BC = OrclDbConnection.getBankConfigFromDB();

                    RV_NeftIn.LocalReport.ReportPath = Server.MapPath("~/Reports/Deposit/neftinward.rdlc");
                    RV_NeftIn.LocalReport.DataSources.Clear();
                    RV_NeftIn.KeepSessionAlive = true;
                    RV_NeftIn.AsyncRendering = true;

                    var prp = new p_report_param();
                    prp.from_dt = Convert.ToDateTime(Request.QueryString["from_dt"]);
                    prp.to_dt = Convert.ToDateTime(Request.QueryString["to_dt"]);
                    prp.brn_cd = Request.QueryString["brn_cd"];

                    string brn_name = _masterLL.GetBranchMaster(prp.brn_cd);

                    List<neft_inward> neftInward = _DepositLL.NeftInward(prp);
                    if (neftInward.Any())
                    {

                        dataSet = Extension.ToDataSet(neftInward);

                    ReportDataSource rdc = new ReportDataSource("NeftInward", dataSet.Tables[0]);
                    ReportParameter[] paramss = new ReportParameter[4];
                    paramss[0] = new ReportParameter("p_bank_name", BC.bank_desc, false);
                    paramss[1] = new ReportParameter("p_branch_name", brn_name, false);
                    paramss[2] = new ReportParameter("p_from_dt", Request.QueryString["from_dt"], false);
                    paramss[3] = new ReportParameter("p_to_dt", Request.QueryString["to_dt"], false);

                    RV_NeftIn.LocalReport.SetParameters(paramss);
                    RV_NeftIn.LocalReport.DataSources.Add(rdc);
                    RV_NeftIn.LocalReport.Refresh();

                }
                    else
                {
                        RV_NeftIn.Visible = false;
                    NoDataFound.Visible = true;
                }

            }
                catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                    RV_NeftIn.Visible = false;
                NoDataFound.Visible = true;
            }
        }

        }
    }
}
