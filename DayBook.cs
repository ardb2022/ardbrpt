using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RDLCReportServer
{
    public class DayBook
    {
       public int srl_no {get;set;}
       public string dr_acc_cd {get;set;}
  public string dr_particulars {get;set;}
    public decimal dr_amt {get;set;}
public string cr_acc_cd {get;set;}
  public string cr_particulars {get;set;}
  public decimal cr_amt {get;set;}
  public decimal cr_amt_tr {get;set;}
  public decimal dr_amt_tr {get;set;}
    }
}