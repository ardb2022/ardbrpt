using Oracle.DataAccess.Client;
using RDLCReportServer.Util;
using SBWSFinanceApi.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace SBWSFinanceApi.DL
{
    public class TradingAc
    {
        string _statement;
        internal List<tt_trading_account> PopulateTradingAc(p_report_param prp)
        {
            List<tt_trading_account> tcaRet=new List<tt_trading_account>();
            string _alter="ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _query= "p_trading_account_rep";
            string _query1= "SELECT ACC_TYPE,"
                          + " ACC_CD,"
                          + " AMOUNT,"
                          + " TYPE,"
                          + " ACC_NAME,"
                          + " SCHEDULE_CD,"
                          + " SUB_SCHEDULE_CD "
                          + " TRADING_FLAG "
                          + " FROM TT_TRADING_ACCOUNT";
            using (var connection = OrclDbConnection.NewConnection)
            {   
                using (var transaction = connection.BeginTransaction())
                {   
                    try{    
                            using (var command = OrclDbConnection.Command(connection, _alter))
                            {
                                    command.ExecuteNonQuery();
                             }     
                            _statement = string.Format(_query);
                            using (var command = OrclDbConnection.Command(connection, _statement))
                            {
                                    command.CommandType = System.Data.CommandType.StoredProcedure;
                                    var parm1 = new OracleParameter("adt_from_dt", OracleDbType.Date, ParameterDirection.Input);
                                    parm1.Value = prp.from_dt;
                                    command.Parameters.Add(parm1);
                                    var parm2 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                                    parm2.Value = prp.to_dt;
                                    command.Parameters.Add(parm2);
                                   var parm3 = new OracleParameter("AS_BRN_CD", OracleDbType.Int32, ParameterDirection.Input);
                                   parm3.Value = prp.brn_cd;
                                   command.Parameters.Add(parm3);
                            command.ExecuteNonQuery();
                                    //transaction.Commit();
                            }
                             _statement = string.Format(_query1);
                            using (var command = OrclDbConnection.Command(connection, _statement))
                            {
                                using (var reader = command.ExecuteReader())
                                {
                                    if (reader.HasRows)     
                                    {
                                        while (reader.Read())
                                        {                                
                                                var tca = new tt_trading_account();
                                                tca.acc_type = UtilityM.CheckNull<string>(reader["ACC_TYPE"]);
                                                tca.acc_cd = UtilityM.CheckNull<int>(reader["ACC_CD"]);
                                                tca.amount = UtilityM.CheckNull<decimal>(reader["AMOUNT"]);
                                                tca.type = UtilityM.CheckNull<string>(reader["TYPE"]);
                                                tca.acc_name = UtilityM.CheckNull<string>(reader["ACC_NAME"]);
                                                tca.schedule_cd = UtilityM.CheckNull<int>(reader["SCHEDULE_CD"]);
                                                //tca.sub_schedule_cd = UtilityM.CheckNull<int>(reader["SUB_SCHEDULE_CD"]);
                                                //tca.trading_flag = UtilityM.CheckNull<string>(reader["TRADING_FLAG"]);
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