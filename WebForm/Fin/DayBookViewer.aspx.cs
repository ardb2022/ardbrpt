using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using RDLCReportServer.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.DataAccess.Client;
using System.Web.Configuration;
using System.Data.SqlClient;
using SBWSFinanceApi.Models;
using SBWSFinanceApi.LL;

namespace RDLCReportServer
{
    public partial class DayBookViewer : System.Web.UI.Page
    {
        private DataSet dataSet;
        public void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    NoDataFound.Visible = false;
                    FinanceReportLL _FinanceReportLL = new FinanceReportLL();
                    BankConfigMstLL _masterLL = new BankConfigMstLL();
                    BankConfig BC = OrclDbConnection.getBankConfigFromDB();
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/Fin/DayBook.rdlc");
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.KeepSessionAlive = true;
                    ReportViewer1.AsyncRendering = false;
                    var prp = new p_report_param();
                    prp.brn_cd = Request.QueryString["brn_cd"];
                    string brn_name = _masterLL.GetBranchMaster(prp.brn_cd);
                    prp.from_dt = Convert.ToDateTime(Request.QueryString["from_dt"]);
                    prp.to_dt = Convert.ToDateTime(Request.QueryString["to_dt"]);
                    prp.acc_cd = Convert.ToInt32(Request.QueryString["acc_cd"]);

                    List<tt_cash_account> DailyCashBook= _FinanceReportLL.PopulateDailyCashBook(prp);
                    if (DailyCashBook.Any())
                    {
                        dataSet = Extension.ToDataSet(DailyCashBook);
                    ReportDataSource rdc = new ReportDataSource("DayBook", dataSet.Tables[0]);
                    ReportParameter[] paramss = new ReportParameter[4];
                    paramss[0] = new ReportParameter("p_bank_name", BC.bank_desc, false);
                    paramss[1] = new ReportParameter("p_branch_name", brn_name, false);
                    paramss[2] = new ReportParameter("p_from_dt", Request.QueryString["from_dt"], false);
                    paramss[3] = new ReportParameter("p_to_dt", Request.QueryString["to_dt"], false);
                    ReportViewer1.LocalReport.SetParameters(paramss);
                    ReportViewer1.LocalReport.DataSources.Add(rdc);
                    ReportViewer1.LocalReport.Refresh();
                }
                    else
                {
                        ReportViewer1.Visible = false;
                    NoDataFound.Visible = true;
                }

            }
                catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                    ReportViewer1.Visible = false;
                NoDataFound.Visible = true;
            }
        }

        }
        

    }
}