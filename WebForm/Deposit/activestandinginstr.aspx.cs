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

namespace RDLCReportServer.WebForm.Deposit
{
    public partial class activestandinginstr : System.Web.UI.Page
    {
        private DataSet dataSet;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                try
                {
                    //  http://localhost:63011/WebForm/Deposit/activestandinginstr?brn_cd=101

                    NoDataFound.Visible = false;
                    DepositLL _DepositLL = new DepositLL();
                    BankConfigMstLL _masterLL = new BankConfigMstLL();
                    BankConfig BC = OrclDbConnection.getBankConfigFromDB();
                    RV_ActiveStandingInstr.LocalReport.ReportPath = Server.MapPath("~/Reports/Deposit/ActiveStandingInstr.rdlc");
                    RV_ActiveStandingInstr.LocalReport.DataSources.Clear();
                    RV_ActiveStandingInstr.KeepSessionAlive = true;
                    RV_ActiveStandingInstr.AsyncRendering = true;

                    var prp = new p_report_param();
                    prp.brn_cd = Request.QueryString["brn_cd"];
                    string brn_name = _masterLL.GetBranchMaster(prp.brn_cd);

                    List<active_standing_instr> activeStandingInstr = _DepositLL.GetActiveStandingInstr(prp);
                    if (activeStandingInstr.Any())
                    {
                        List<mm_acc_type> category = _masterLL.GetAccountTypeMaster();
                    List<mm_constitution> constitution = _masterLL.GetConstitution();

                    foreach (var x in activeStandingInstr)
                    {
                        var filtCat1 = category.FirstOrDefault(y => y.acc_type_cd == x.acc_type_from);
                        var filtCat2 = category.FirstOrDefault(y => y.acc_type_cd == x.acc_type_to);

                        if (filtCat1 != null && filtCat1.acc_type_cd > 0)
                        {
                            x.acc_type_from_desc = filtCat1.acc_type_desc;
                        }

                        if (filtCat2 != null && filtCat2.acc_type_cd > 0)
                        {
                            x.acc_type_to_desc = filtCat2.acc_type_desc;
                        }
                    }

                    dataSet = Extension.ToDataSet(activeStandingInstr);
                    ReportDataSource rdc = new ReportDataSource("ActiveStandingInstr", dataSet.Tables[0]);
                    ReportParameter[] paramss = new ReportParameter[3];
                    paramss[0] = new ReportParameter("p_bank_name", BC.bank_name, false);
                    paramss[1] = new ReportParameter("p_branch_name", brn_name, false);
                    paramss[2] = new ReportParameter("p_branch_code", prp.brn_cd, false);


                    RV_ActiveStandingInstr.LocalReport.SetParameters(paramss);
                    RV_ActiveStandingInstr.LocalReport.DataSources.Add(rdc);
                    RV_ActiveStandingInstr.LocalReport.Refresh();
                }
                else
                {
                    RV_ActiveStandingInstr.Visible = false;
                    NoDataFound.Visible = true;
                }

            }
                catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                RV_ActiveStandingInstr.Visible = false;
                NoDataFound.Visible = true;
            }
        }


        }
    }
}
