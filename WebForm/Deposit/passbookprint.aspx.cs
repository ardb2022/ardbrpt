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
    public partial class passbookprint : System.Web.UI.Page
    {
        private DataSet dataSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    //  http://localhost:63011/WebForm/Deposit/passbookprint?brn_cd=101&acc_type_cd=1&acc_num=101604068&from_dt=01/01/2018&to_dt=12/12/2020
                    NoDataFound.Visible = false;
                    DepositLL _DepositLL = new DepositLL();
                    BankConfigMstLL _masterLL = new BankConfigMstLL();
                    //BankConfig BC = OrclDbConnection.getBankConfigFromDB();

                    RV_Passbook.LocalReport.ReportPath = Server.MapPath("~/Reports/Deposit/passbookprint.rdlc");
                    RV_Passbook.LocalReport.DataSources.Clear();
                    RV_Passbook.KeepSessionAlive = true;
                    RV_Passbook.AsyncRendering = true;

                    var prp = new p_report_param();
                    prp.from_dt = Convert.ToDateTime(Request.QueryString["from_dt"]);
                    prp.from_dt = Convert.ToDateTime(prp.from_dt.ToString("dd/MM/yyyy"));

                    prp.to_dt = Convert.ToDateTime(Request.QueryString["to_dt"]);
                    prp.to_dt = Convert.ToDateTime(prp.to_dt.ToString("dd/MM/yyyy")).AddHours(23).AddMinutes(59).AddSeconds(59);

                    prp.brn_cd = Request.QueryString["brn_cd"];
                    prp.acc_num = Request.QueryString["acc_num"];
                    prp.acc_type_cd = Convert.ToInt16( Request.QueryString["acc_type_cd"]);

                    string brn_name = _masterLL.GetBranchMaster(prp.brn_cd);

                    List<passbook_print> passbookPrint = _DepositLL.PassBookPrint(prp);
                    if (passbookPrint.Any())
                    {
                        dataSet = Extension.ToDataSet(passbookPrint);

                    ReportDataSource rdc = new ReportDataSource("PassBook", dataSet.Tables[0]);
                    RV_Passbook.LocalReport.DataSources.Add(rdc);
                    RV_Passbook.LocalReport.Refresh();

                }
                    else
                {
                        RV_Passbook.Visible = false;
                    NoDataFound.Visible = true;
                }

            }
                catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                    RV_Passbook.Visible = false;
                NoDataFound.Visible = true;
            }
        }

        }
    }
}
