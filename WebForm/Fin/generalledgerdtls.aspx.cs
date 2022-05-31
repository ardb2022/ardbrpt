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
    public partial class generalledgerdtls : System.Web.UI.Page
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
                    BankConfig BC = OrclDbConnection.getBankConfigFromDB();
                    RV_gld.LocalReport.ReportPath = Server.MapPath("~/Reports/Fin/generalledgerdtls.rdlc");
                    RV_gld.LocalReport.DataSources.Clear();
                    RV_gld.KeepSessionAlive = false;
                    RV_gld.AsyncRendering = false;
                    var prp = new p_report_param();
                     prp.brn_cd = Request.QueryString["brn_cd"];
                    string brn_name = _masterLL.GetBranchMaster(prp.brn_cd);
                    prp.from_dt = Convert.ToDateTime(Request.QueryString["adt_from_dt"]);
                     prp.to_dt = Convert.ToDateTime(Request.QueryString["adt_to_dt"]);
                     prp.ad_from_acc_cd = Convert.ToInt32(Request.QueryString["ad_from_acc_cd"]);
                    prp.ad_to_acc_cd = Convert.ToInt32(Request.QueryString["ad_to_acc_Cd"]);
                    List<tt_gl_trans> GeneralLedger = _FinanceReportLL.getGeneralLedgerTransactionDtls(prp, true);
                    if (GeneralLedger.Any())
                    {
                        List<m_acc_master> accmaster = _masterLL.GetAccountMaster();
                    GeneralLedger.ForEach(x => x.trans_year_month = x.trans_month.ToString() + "/" + x.trans_year.ToString());
                    foreach (var x in GeneralLedger)
                    {
                        //string y = "";
                        var filtCat = accmaster.FirstOrDefault(y => y.acc_cd == x.acc_cd);
                        if (filtCat != null && filtCat.acc_cd > 0)
                        {
                            x.acc_cd_desc = x.acc_cd + "-" + filtCat.acc_name;
                        }
                        else
                        {
                            x.acc_cd_desc = x.acc_cd + "- NA";
                        }
                    }
                    dataSet = Extension.ToDataSet(GeneralLedger);
                    ReportDataSource rdc = new ReportDataSource("generalledgerdtls", dataSet.Tables[0]);
                    ReportParameter[] paramss = new ReportParameter[4];
                    paramss[0] = new ReportParameter("p_bank_name", BC.bank_desc, false);
                    paramss[1] = new ReportParameter("p_branch_name", brn_name, false);
                    paramss[2] = new ReportParameter("p_from_dt", Request.QueryString["adt_from_dt"], false);
                    paramss[3] = new ReportParameter("p_to_dt", Request.QueryString["adt_to_dt"], false);
                    RV_gld.LocalReport.SetParameters(paramss);
                    RV_gld.LocalReport.DataSources.Add(rdc);
                    RV_gld.LocalReport.Refresh();
                }
                    else
                {
                        RV_gld.Visible = false;
                    NoDataFound.Visible = true;
                }

            }
                catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                    RV_gld.Visible = false;
                NoDataFound.Visible = true;
            }
        }
        }
    }
}