
using System;
using System.Collections.Generic;
using SBWSFinanceApi.DL;
using SBWSFinanceApi.Models;

namespace SBWSFinanceApi.LL
{
    public class VoucherLL
    {
        VoucherDL _dac = new VoucherDL();
        internal List<t_voucher_dtls> GetTVoucherDtls(t_voucher_dtls tvd)
        {

            return _dac.GetTVoucherDtls(tvd);
        }
        VoucherPrintDL _dacPrint = new VoucherPrintDL();
        internal List<t_voucher_narration> GetTVoucherDtlsForPrint(t_voucher_dtls tvd)
        {

            return _dacPrint.GetTVoucherDtlsForPrint(tvd);
        }
    }
}


// SS