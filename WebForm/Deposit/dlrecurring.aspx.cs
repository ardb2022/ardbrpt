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
    public partial class dlrecurring : System.Web.UI.Page
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
                    RV_DLR.LocalReport.ReportPath = Server.MapPath("~/Reports/Deposit/dlrecurring.rdlc");
                    RV_DLR.LocalReport.DataSources.Clear();
                    RV_DLR.KeepSessionAlive = true;
                    RV_DLR.AsyncRendering = true;
                    var prp = new p_report_param();
                    prp.from_dt = Convert.ToDateTime(Request.QueryString["from_dt"]);
                    prp.brn_cd = Request.QueryString["brn_cd"];
                    prp.acc_type_cd = 6;
                    List<tt_sbca_dtl_list> depositdetails = _DepositLL.PopulateDLRecuring(prp);
                    if (depositdetails.Any())
                    {
                    string brn_name = _masterLL.GetBranchMaster(prp.brn_cd);
                    List<mm_constitution> constitution = _masterLL.GetConstitution();
                    foreach (var x in depositdetails)
                    {
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
                    ReportDataSource rdc = new ReportDataSource("dlrecurring", dataSet.Tables[0]);
                    ReportParameter[] paramss = new ReportParameter[3];
                    paramss[0] = new ReportParameter("p_bank_name", BC.bank_desc, false);
                    paramss[1] = new ReportParameter("p_branch_name", brn_name, false);
                    paramss[2] = new ReportParameter("p_from_dt", Request.QueryString["from_dt"], false);
                    RV_DLR.LocalReport.SetParameters(paramss);
                    RV_DLR.LocalReport.DataSources.Add(rdc);
                    RV_DLR.LocalReport.Refresh();


                }
                    else
                {
                    RV_DLR.Visible = false;
                    NoDataFound.Visible = true;
                }

            }
                catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                RV_DLR.Visible = false;
                NoDataFound.Visible = true;
            }
        }

        }
    }
}