using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using RDLCReportServer.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
                    Task t = new Task(getdatasetAsync);
                    t.Start();
                    t.Wait();
                    //getdatasetAsync();
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/DayBook.rdlc");
                    ReportViewer1.LocalReport.DataSources.Clear();
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }
        public async void getdatasetAsync()
        {
            var url = "http://36.255.3.143/VCCB/api/Report/PopulateDailyCashBook";
            var client = new HttpClient();
            var prp = new ReportParam();
            prp.brn_cd = "101";
            prp.from_dt = DateTime.Today.AddDays(-700); ;
            prp.to_dt = DateTime.Now;
            prp.acc_cd = 28101;
            var json = JsonConvert.SerializeObject(prp);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, data);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                List<DayBook> DayList = JsonConvert.DeserializeObject<List<DayBook>>(jsonString);
                dataSet = Extension.ToDataSet(DayList);
                ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DayBook", dataSet.Tables[0]));
                ReportViewer1.LocalReport.Refresh();

            }
            // dataSet.CreateDataTable<DayBook>();
            // dataSet = JsonConvert.DeserializeObject<DataSet>(jsonString);
            // UserList.ToDataTable<User>();
            //Console.WriteLine(result);

        }

    }
}