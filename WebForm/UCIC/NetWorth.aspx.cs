using Microsoft.Reporting.WebForms;
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

namespace RDLCReportServer.WebForm.UCIC
{
    public partial class NetWorth : System.Web.UI.Page
    {
        private DataSet dataSet1;
        private DataSet dataSet2;
        private DataSet dataSet3;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    NoDataFound.Visible = false;
                    CustomerLL _CustomerLL = new CustomerLL();
                    BankConfigMstLL _masterLL = new BankConfigMstLL();
                    BankConfig BC = OrclDbConnection.getBankConfigFromDB();
                    RV_networth.LocalReport.ReportPath = Server.MapPath("~/Reports/UCIC/networth.rdlc");
                    
                    RV_networth.KeepSessionAlive = false;
                    RV_networth.AsyncRendering = false;
                    var prp = new mm_customer();
                    prp.brn_cd = Request.QueryString["brn_cd"];
                    prp.cust_cd = Convert.ToInt64(Request.QueryString["cust_cd"]) ;
                    List<mm_customer> memberdetails = _CustomerLL.GetCustShortDtls(prp);
                    if (memberdetails.Any())
                    {
                        List<tm_deposit> depositdetails = _CustomerLL.GetDepositDtls(prp);
                    List<tm_loan_all> loandetails = _CustomerLL.GetLoanDtls(prp);
                    List<mm_acc_type> category = _masterLL.GetAccountTypeMaster();
                        string brn_name = _masterLL.GetBranchMaster(prp.brn_cd);

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
                    //memberdetails.ForEach(x => x.catg_desc = category.Where(y => y.catg_cd == x.catg_cd).Select(m=>m.catg_desc).FirstOrDefault().ToString());
                    dataSet1 = Extension.ToDataSet(memberdetails);
                    dataSet2 = Extension.ToDataSet(depositdetails);
                    dataSet3 = Extension.ToDataSet(loandetails);
                    ReportDataSource rdc1 = new ReportDataSource("networth_1", dataSet1.Tables[0]);
                    ReportDataSource rdc2 = new ReportDataSource("networth_2", dataSet2.Tables[0]);
                    ReportDataSource rdc3 = new ReportDataSource("networth_3", dataSet3.Tables[0]);
                    ReportParameter[] paramss = new ReportParameter[3];
                    paramss[0] = new ReportParameter("p_bank_name", BC.bank_name, false);
                    paramss[1] = new ReportParameter("p_branch_name", brn_name, false);
                    paramss[2] = new ReportParameter("p_from_dt", Request.QueryString["from_dt"], false);
                    RV_networth.LocalReport.SetParameters(paramss);
                    RV_networth.LocalReport.DataSources.Clear();
                    RV_networth.LocalReport.DataSources.Add(rdc1);
                    RV_networth.LocalReport.Refresh();
                    RV_networth.LocalReport.DataSources.Add(rdc2);
                    RV_networth.LocalReport.Refresh();
                    RV_networth.LocalReport.DataSources.Add(rdc3);
                    RV_networth.LocalReport.Refresh();


                }
                    else
                {
                    RV_networth.Visible = false;
                    NoDataFound.Visible = true;
                }

            }
                catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                RV_networth.Visible = false;
                NoDataFound.Visible = true;
            }
        }
        }
    }
}