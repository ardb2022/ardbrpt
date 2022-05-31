using RDLCReportServer.Util;
using SBWSFinanceApi.Models;
using System;
using System.Collections.Generic;

namespace SBWSFinanceApi.DL
{
    public class TrialBalanceDL
    {
        string _statement;
        internal List<tt_trial_balance> PopulateTrialBalance(p_report_param prp)
        {
            List<tt_trial_balance> tcaRet=new List<tt_trial_balance>();
            string _query=" SELECT TM_ACC_BALANCE.BALANCE_DT,                  "
    +" TM_ACC_BALANCE.ACC_CD,"
	+" M_ACC_MASTER.ACC_NAME,"
    +" TM_ACC_BALANCE.BALANCE_AMT DEBIT,"
	+" 0.00 CREDIT,"
	+" M_ACC_MASTER.ACC_TYPE"
    +" FROM TM_ACC_BALANCE,"
	+" M_ACC_MASTER"
	+" WHERE TM_ACC_BALANCE.ACC_CD = M_ACC_MASTER.ACC_CD"
    +" AND TM_ACC_BALANCE.BRN_CD ={0}"
	+" AND TM_ACC_BALANCE.BALANCE_DT = to_date('{1}','dd-mm-yyyy' ) "
	+" AND TM_ACC_BALANCE.BALANCE_AMT > 0"
	+" AND TM_ACC_BALANCE.ACC_CD <> {2}"
    +" AND TM_ACC_BALANCE.ACC_CD <>{3}"
	+" UNION"
	+" SELECT TM_ACC_BALANCE.BALANCE_DT,"
    +" TM_ACC_BALANCE.ACC_CD,"
  	+" M_ACC_MASTER.ACC_NAME,"
    +" 0.00 DEBIT,"
    +" ABS(TM_ACC_BALANCE.BALANCE_AMT) CREDIT,"
  	+" M_ACC_MASTER.ACC_TYPE"
    +" FROM TM_ACC_BALANCE,"
  	+" M_ACC_MASTER"
  	+" WHERE TM_ACC_BALANCE.ACC_CD = M_ACC_MASTER.ACC_CD"
    +" AND TM_ACC_BALANCE.BRN_CD = {0}"        
  	+" AND TM_ACC_BALANCE.BALANCE_DT =to_date('{1}','dd-mm-yyyy' )"
  	+" AND TM_ACC_BALANCE.BALANCE_AMT < 0"
  	+" AND TM_ACC_BALANCE.ACC_CD <>{2}"
    +" AND TM_ACC_BALANCE.ACC_CD <>{3}"
    +" ORDER BY 6";
            using (var connection = OrclDbConnection.NewConnection)
            {   
                using (var transaction = connection.BeginTransaction())
                {   
                    try{         
                            _statement = string.Format(_query,
                                            string.IsNullOrWhiteSpace( prp.brn_cd) ? "brn_cd" : string.Concat("'",  prp.brn_cd , "'"),
                                            prp.trial_dt!= null ? prp.trial_dt.ToString("dd/MM/yyyy"): "trial_dt",
                                            prp.pl_acc_cd !=0 ? Convert.ToString(prp.pl_acc_cd) : "pl_acc_cd",
                                            prp.gp_acc_cd !=0 ? Convert.ToString(prp.gp_acc_cd) : "gp_acc_cd"
                                            );
                            using (var command = OrclDbConnection.Command(connection, _statement))
                            {
                                 using (var reader = command.ExecuteReader())
                                {
                                    if (reader.HasRows)     
                                    {
                                        while (reader.Read())
                                        {                                
                                                var tca = new tt_trial_balance();
                                                tca.balance_dt = UtilityM.CheckNull<DateTime>(reader["BALANCE_DT"]);
                                                tca.acc_cd = UtilityM.CheckNull<int>(reader["ACC_CD"]);
                                                tca.acc_name = UtilityM.CheckNull<string>(reader["ACC_NAME"]);
                                                tca.dr = UtilityM.CheckNull<decimal>(reader["DEBIT"]);
                                                tca.cr = UtilityM.CheckNull<decimal>(reader["CREDIT"]);
                                                tca.acc_type = UtilityM.CheckNull<string>(reader["ACC_TYPE"]);
                                                tcaRet.Add(tca);
                                        }
                                    }
                                }
                            }
                            
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            tcaRet=null;
                        }
                }            
            }
            return tcaRet;
    }  
}
}