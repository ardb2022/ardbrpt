using RDLCReportServer.DL.UCIC;
using RDLCReportServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RDLCReportServer.LL.UCIC
{
    public class CustomerLL
    {
        CustomerDL _dac = new CustomerDL();
        internal List<mm_customer> GetCustomerDtls(mm_customer pmc)
        {

            return _dac.GetCustomerDtls(pmc);
        }

        internal List<mm_customer> GetCustShortDtls(mm_customer pmc)
        {

            return _dac.GetCustShortDtls(pmc);
        }

        internal List<tm_deposit> GetDepositDtls(mm_customer pmc)
        {

            return _dac.GetDepositDtls(pmc);
        }

        internal List<tm_loan_all> GetLoanDtls(mm_customer pmc)
        {

            return _dac.GetLoanDtls(pmc);
        }
    }
}