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
    public partial class memberdetails : System.Web.UI.Page
    {
        private DataSet dataSet;
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
                    RVmd.LocalReport.ReportPath = Server.MapPath("~/Reports/UCIC/memberdetails.rdlc");
                    RVmd.LocalReport.DataSources.Clear();
                    RVmd.KeepSessionAlive = false;
                    RVmd.AsyncRendering = false;
                    var prp = new mm_customer();
                    prp.brn_cd = Request.QueryString["brn_cd"];
                    
                    List<mm_customer> memberdetails = _CustomerLL.GetCustomerDtls(prp);
                    if (memberdetails.Any())
                    {
                        string brn_name = _masterLL.GetBranchMaster(prp.brn_cd);
                        List<mm_category> category = _masterLL.GetCategoryMaster();
                    foreach(var x in memberdetails)
                    {
                        //string y = "";
                        var filtCat = category.FirstOrDefault(y => y.catg_cd == x.catg_cd);
                        if (filtCat != null && filtCat.catg_cd > 0)
                        {
                            x.catg_desc = filtCat.catg_desc;
                        } else
                        {
                            x.catg_desc = "NA";
                        }                        
                    }
                    //memberdetails.ForEach(x => x.catg_desc = category.Where(y => y.catg_cd == x.catg_cd).Select(m=>m.catg_desc).FirstOrDefault().ToString());
                    dataSet = Extension.ToDataSet(memberdetails);
                    ReportDataSource rdc = new ReportDataSource("memberdetails", dataSet.Tables[0]);
                    ReportParameter[] paramss = new ReportParameter[3];
                    paramss[0] = new ReportParameter("p_bank_name", BC.bank_name, false);
                    paramss[1] = new ReportParameter("p_branch_name", brn_name, false);
                    paramss[2] = new ReportParameter("p_from_dt", Request.QueryString["from_dt"], false);
                    RVmd.LocalReport.SetParameters(paramss);
                    RVmd.LocalReport.DataSources.Add(rdc);
                    RVmd.LocalReport.Refresh();


                }
                    else
                {
                        RVmd.Visible = false;
                    NoDataFound.Visible = true;
                }

            }
                catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                    RVmd.Visible = false;
                NoDataFound.Visible = true;
            }
        }
        }
    }
}