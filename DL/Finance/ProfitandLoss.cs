using Oracle.DataAccess.Client;
using RDLCReportServer.Util;
using SBWSFinanceApi.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace SBWSFinanceApi.DL
{
    public class ProfitandLoss
    {
        string _statement;
        internal List<tt_pl_book> PopulateProfitandLoss(p_report_param prp)
        {
            List<tt_pl_book> tcaRet=new List<tt_pl_book>();
            string _alter="ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _query= "p_pl_scroll_brn";
            string _query1= "SELECT SL_NO,"
                          + " CR_ACC_CD,"
                          + " CR_AMOUNT,"
                          + " DR_ACC_CD,"
                          + " DR_AMOUNT,"
                          + " SCH_CD,"
                          + " SCH_CD_CR "
                          + " FROM TT_PL_BOOK";
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
                                    var parm1 = new OracleParameter("as_brn_cd", OracleDbType.Int32, ParameterDirection.Input);
                                    parm1.Value = prp.brn_cd;
                                    command.Parameters.Add(parm1);
                                    var parm2 = new OracleParameter("adt_dt", OracleDbType.Date, ParameterDirection.Input);
                                    parm2.Value = prp.from_dt;
                                    command.Parameters.Add(parm2);
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
                                                var tca = new tt_pl_book();
                                     
                                                tca.cr_acc_cd = UtilityM.CheckNull<decimal>(reader["CR_ACC_CD"]);
                                                tca.cr_amount = UtilityM.CheckNull<decimal>(reader["CR_AMOUNT"]);
                                                tca.dr_acc_cd = UtilityM.CheckNull<decimal>(reader["DR_ACC_CD"]);
                                                tca.dr_amount = UtilityM.CheckNull<decimal>(reader["DR_AMOUNT"]);
                                               // tca.sch_cd = UtilityM.CheckNull<int>(reader["SCH_CD"]);
                                               // tca.sch_cd_cr = UtilityM.CheckNull<int>(reader["SCH_CD_CR"]);
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