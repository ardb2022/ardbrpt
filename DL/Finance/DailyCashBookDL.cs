using Oracle.DataAccess.Client;
using RDLCReportServer.Util;
using SBWSFinanceApi.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace SBWSFinanceApi.DL
{
    public class DailyCashBookDL
    {
        string _statement;
        //New Line
        internal List<tt_cash_account> PopulateDailyCashBook(p_report_param prp)
        {
            List<tt_cash_account> tcaRet=new List<tt_cash_account>();
            string _query="P_CASH_BOOK_REP";
            string _query1=" SELECT TT_CASH_ACCOUNT.SRL_NO,TT_CASH_ACCOUNT.DR_ACC_CD,TT_CASH_ACCOUNT.DR_PARTICULARS,TT_CASH_ACCOUNT.DR_AMT,"
                            +" TT_CASH_ACCOUNT.CR_ACC_CD,TT_CASH_ACCOUNT.CR_PARTICULARS,TT_CASH_ACCOUNT.CR_AMT,TT_CASH_ACCOUNT.CR_AMT_TR,TT_CASH_ACCOUNT.DR_AMT_TR"
                            +" FROM TT_CASH_ACCOUNT ";
            using (var connection = OrclDbConnection.NewConnection)
            {   
                using (var transaction = connection.BeginTransaction())
                {   
                    try{         
                            _statement = string.Format(_query);
                            using (var command = OrclDbConnection.Command(connection, _statement))
                            {
                                    command.CommandType = System.Data.CommandType.StoredProcedure;
                                    var parm1 = new OracleParameter("ad_cash_acc_cd", OracleDbType.Int32, ParameterDirection.Input);
                                    parm1.Value = prp.acc_cd;
                                    command.Parameters.Add(parm1);
                                    var parm2 = new OracleParameter("adt_from_dt", OracleDbType.Date, ParameterDirection.Input);
                                    parm2.Value = prp.from_dt;
                                    command.Parameters.Add(parm2);
                                     var parm3 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                                    parm3.Value = prp.to_dt;
                                    command.Parameters.Add(parm3);
                                    var parm4 = new OracleParameter("as_brn_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                                    parm4.Value = prp.brn_cd;
                                    command.Parameters.Add(parm4);
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
                                                var tca = new tt_cash_account();
                                                tca.srl_no = UtilityM.CheckNull<int>(reader["SRL_NO"]);
                                                tca.dr_acc_cd = UtilityM.CheckNull<int>(reader["DR_ACC_CD"]);
                                                tca.dr_particulars = UtilityM.CheckNull<string>(reader["DR_PARTICULARS"]);
                                                tca.dr_amt = UtilityM.CheckNull<decimal>(reader["DR_AMT"]);
                                                tca.cr_acc_cd = UtilityM.CheckNull<int>(reader["CR_ACC_CD"]);
                                                tca.cr_particulars = UtilityM.CheckNull<string>(reader["CR_PARTICULARS"]);
                                                tca.cr_amt = UtilityM.CheckNull<decimal>(reader["CR_AMT"]);
                                                tca.cr_amt_tr = UtilityM.CheckNull<decimal>(reader["CR_AMT_TR"]);
                                                tca.dr_amt_tr = UtilityM.CheckNull<decimal>(reader["DR_AMT_TR"]);
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
        internal List<tt_cash_account> PopulateDailyCashAccount(p_report_param prp)
        {
            List<tt_cash_account> tcaRet = new List<tt_cash_account>();
            string _query = "P_CASH_ACCOUNT_REP";
            string _query1 = " SELECT TT_CASH_ACCOUNT.SRL_NO,TT_CASH_ACCOUNT.DR_ACC_CD,TT_CASH_ACCOUNT.DR_PARTICULARS,TT_CASH_ACCOUNT.DR_AMT,"
                            + " TT_CASH_ACCOUNT.CR_ACC_CD,TT_CASH_ACCOUNT.CR_PARTICULARS,TT_CASH_ACCOUNT.CR_AMT,TT_CASH_ACCOUNT.CR_AMT_TR,TT_CASH_ACCOUNT.DR_AMT_TR"
                            + " FROM TT_CASH_ACCOUNT ";
            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _statement = string.Format(_query);
                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            var parm1 = new OracleParameter("ad_cash_acc_cd", OracleDbType.Int32, ParameterDirection.Input);
                            parm1.Value = prp.acc_cd;
                            command.Parameters.Add(parm1);
                            var parm2 = new OracleParameter("adt_from_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm2.Value = prp.from_dt;
                            command.Parameters.Add(parm2);
                            var parm3 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.to_dt;
                            command.Parameters.Add(parm3);
                            var parm4 = new OracleParameter("as_brn_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm4.Value = prp.brn_cd;
                            command.Parameters.Add(parm4);
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
                                        var tca = new tt_cash_account();
                                        tca.srl_no = UtilityM.CheckNull<int>(reader["SRL_NO"]);
                                        tca.dr_acc_cd = UtilityM.CheckNull<int>(reader["DR_ACC_CD"]);
                                        tca.dr_particulars = UtilityM.CheckNull<string>(reader["DR_PARTICULARS"]);
                                        tca.dr_amt = UtilityM.CheckNull<decimal>(reader["DR_AMT"]);
                                        tca.cr_acc_cd = UtilityM.CheckNull<int>(reader["CR_ACC_CD"]);
                                        tca.cr_particulars = UtilityM.CheckNull<string>(reader["CR_PARTICULARS"]);
                                        tca.cr_amt = UtilityM.CheckNull<decimal>(reader["CR_AMT"]);
                                        tca.cr_amt_tr = UtilityM.CheckNull<decimal>(reader["CR_AMT_TR"]);
                                        tca.dr_amt_tr = UtilityM.CheckNull<decimal>(reader["DR_AMT_TR"]);
                                        tcaRet.Add(tca);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        tcaRet = null;
                    }
                }
            }
            return tcaRet;
        }
    }
}