using Oracle.DataAccess.Client;
using RDLCReportServer.Model;
using RDLCReportServer.Util;
using SBWSFinanceApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RDLCReportServer.DL.Deposit
{
    public class DepositDL
    {
        string _statement;
        internal List<tt_dep_sub_cash_book> PopulateSubCashBookDeposit(p_report_param prp)
        {
            List<tt_dep_sub_cash_book> tcaRet = new List<tt_dep_sub_cash_book>();
            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _query = "p_sub_csh_bk_dep";
            string _query1 = " SELECT 'PRINCIPAL' PRN_INTT_FLAG,"
+ " TT_DEP_SUB_CASH_BOOK.ACC_TYPE_CD,"
+ " TT_DEP_SUB_CASH_BOOK.ACC_NUM,    "
+ " TT_DEP_SUB_CASH_BOOK.CASH_DR,    "
+ " TT_DEP_SUB_CASH_BOOK.TRF_DR,     "
+ " TT_DEP_SUB_CASH_BOOK.CASH_CR,    "
+ " TT_DEP_SUB_CASH_BOOK.TRF_CR,     "
+ " TT_DEP_SUB_CASH_BOOK.BAL_AMT,    "
+ " TT_DEP_SUB_CASH_BOOK.CONSTITUTION_CD,"
+ " TT_DEP_SUB_CASH_BOOK.CUST_NAME, "
+ " TT_DEP_SUB_CASH_BOOK.BRN_CD"
+ " FROM TT_DEP_SUB_CASH_BOOK  "
+ " WHERE NVL(TT_DEP_SUB_CASH_BOOK.CATG_CD, '0') NOT IN ('1', '2') "
+ " UNION ALL "
+ " SELECT 'INTEREST' PRN_INTT_FLAG, "
+ " TT_DEP_SUB_CASH_BOOK.ACC_TYPE_CD,"
+ " TT_DEP_SUB_CASH_BOOK.ACC_NUM,    "
+ " TT_DEP_SUB_CASH_BOOK.CASH_DR,    "
+ " TT_DEP_SUB_CASH_BOOK.TRF_DR,     "
+ " TT_DEP_SUB_CASH_BOOK.CASH_CR,    "
+ " TT_DEP_SUB_CASH_BOOK.TRF_CR,     "
+ " TT_DEP_SUB_CASH_BOOK.BAL_AMT,    "
+ " TT_DEP_SUB_CASH_BOOK.CONSTITUTION_CD, "
+ " TT_DEP_SUB_CASH_BOOK.CUST_NAME,"
+ " TT_DEP_SUB_CASH_BOOK.BRN_CD    "
+ " FROM TT_DEP_SUB_CASH_BOOK "
+ " WHERE    NVL(TT_DEP_SUB_CASH_BOOK.CATG_CD, '0') IN ('1', '2')";
            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
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
                            var parm2 = new OracleParameter("adt_as_on_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm2.Value = prp.from_dt;
                            command.Parameters.Add(parm2);
                            command.ExecuteNonQuery();
                            transaction.Commit();
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
                                        var tca = new tt_dep_sub_cash_book();
                                        tca.prn_intt_flag = UtilityM.CheckNull<string>(reader["PRN_INTT_FLAG"]);
                                        tca.acc_type_cd = UtilityM.CheckNull<int>(reader["ACC_TYPE_CD"]);
                                        tca.acc_num = UtilityM.CheckNull<string>(reader["ACC_NUM"]);
                                        tca.cash_dr = UtilityM.CheckNull<decimal>(reader["CASH_DR"]);
                                        tca.trf_dr = UtilityM.CheckNull<decimal>(reader["TRF_DR"]);
                                        tca.cash_cr = UtilityM.CheckNull<decimal>(reader["CASH_CR"]);
                                        tca.trf_cr = UtilityM.CheckNull<decimal>(reader["TRF_CR"]);
                                        tca.bal_amt = UtilityM.CheckNull<decimal>(reader["BAL_AMT"]);
                                        tca.constitution_cd = UtilityM.CheckNull<int>(reader["CONSTITUTION_CD"]);
                                        tca.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                        tca.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
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

        internal List<tt_sbca_dtl_list> PopulateDLSavings(p_report_param prp)
        {
            List<tt_sbca_dtl_list> tcaRet = new List<tt_sbca_dtl_list>();
            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _query = "p_sbca_detailed_list";
            string _query1 = " SELECT TT_SBCA_DTL_LIST.BRN_CD,    "
+ " TT_SBCA_DTL_LIST.ACC_TYPE_CD,      "
+ " TT_SBCA_DTL_LIST.ACC_NUM,          "
+ " TT_SBCA_DTL_LIST.CUST_NAME,        "
+ " TT_SBCA_DTL_LIST.OPENING_DT,       "
+ " TT_SBCA_DTL_LIST.CONSTITUTION_DESC,"
+ " TT_SBCA_DTL_LIST.BALANCE,          "
+ " TT_SBCA_DTL_LIST.INTT_PROV_AMT, "
+ " 1 rowmark, "
+ " MM_CUSTOMER.GUARDIAN_NAME,   "
+ " MM_CUSTOMER.PRESENT_ADDRESS, "
+ " MM_CUSTOMER.DT_OF_BIRTH, "
+ " nvl(MM_CUSTOMER.AGE,0) AGE,  "
+ " TM_DEPOSIT.CUST_CD  CUST_CD "
+ " FROM TT_SBCA_DTL_LIST,   "
+ " TM_DEPOSIT, "
+ " MM_CUSTOMER "
+ " WHERE ( TT_SBCA_DTL_LIST.ACC_TYPE_CD = TM_DEPOSIT.ACC_TYPE_CD ) and "
+ " (TT_SBCA_DTL_LIST.ACC_NUM = TM_DEPOSIT.ACC_NUM ) and "
+ " (TM_DEPOSIT.CUST_CD = MM_CUSTOMER.CUST_CD) ";
            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = OrclDbConnection.Command(connection, _alter))
                        {
                            command.ExecuteNonQuery();
                        }
                        _statement = string.Format(_query);
                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            var parm1 = new OracleParameter("brncd", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm1.Value = prp.brn_cd;
                            command.Parameters.Add(parm1);
                            var parm2 = new OracleParameter("acctypecd", OracleDbType.Int16, ParameterDirection.Input);
                            parm2.Value = prp.acc_type_cd;
                            command.Parameters.Add(parm2);
                            var parm3 = new OracleParameter("constcd", OracleDbType.Int32, ParameterDirection.Input);
                            parm3.Value = prp.const_cd;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("adt_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm4.Value = prp.from_dt;
                            command.Parameters.Add(parm4);

                            command.ExecuteNonQuery();
                            transaction.Commit();
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
                                        var tca = new tt_sbca_dtl_list();
                                        tca.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                        tca.acc_type_cd = UtilityM.CheckNull<int>(reader["ACC_TYPE_CD"]);
                                        tca.acc_num = UtilityM.CheckNull<string>(reader["ACC_NUM"]);
                                        tca.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                        tca.opening_dt = UtilityM.CheckNull<DateTime>(reader["OPENING_DT"]);
                                        tca.constitution_desc = UtilityM.CheckNull<string>(reader["CONSTITUTION_DESC"]);
                                        tca.balance = UtilityM.CheckNull<decimal>(reader["BALANCE"]);
                                        tca.intt_prov_amt = UtilityM.CheckNull<decimal>(reader["INTT_PROV_AMT"]);
                                        tca.guardian_name = UtilityM.CheckNull<string>(reader["GUARDIAN_NAME"]);
                                        tca.present_address = UtilityM.CheckNull<string>(reader["PRESENT_ADDRESS"]);
                                        tca.dt_of_birth = UtilityM.CheckNull<DateTime>(reader["DT_OF_BIRTH"]);
                                        tca.age = UtilityM.CheckNull<decimal>(reader["AGE"]);
                                        tca.cust_cd = UtilityM.CheckNull<decimal>(reader["CUST_CD"]);

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

        internal List<tt_sbca_dtl_list> PopulateDLRecuring(p_report_param prp)
        {
            List<tt_sbca_dtl_list> tcaRet = new List<tt_sbca_dtl_list>();
            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _query = "p_rd_prov_intt_detail_list";
            string _query1 = " SELECT TT_RDPROV_INTT.ACC_TYPE_CD,"
+ " TT_RDPROV_INTT.ACC_NUM,  "
+ " TT_RDPROV_INTT.CONSTITUTION_CD,   "
+ " TT_RDPROV_INTT.CUST_NAME,  "
+ " TT_RDPROV_INTT.OPENING_DT, "
+ " TT_RDPROV_INTT.MAT_DT,     "
+ " TT_RDPROV_INTT.INSTL_AMT,  "
+ " TT_RDPROV_INTT.INTT_RT,       "
+ " TT_RDPROV_INTT.PRN_AMT,       "
+ " TT_RDPROV_INTT.PROV_INTT_AMT, "
+ " TM_DEPOSIT.CUST_CD   "
+ " FROM TT_RDPROV_INTT, "
+ " TM_DEPOSIT "
+ " WHERE ( TT_RDPROV_INTT.ACC_TYPE_CD = TM_DEPOSIT.ACC_TYPE_CD ) and"
+ " ( TT_RDPROV_INTT.ACC_NUM = TM_DEPOSIT.ACC_NUM ) and "
+ " ( ( TT_RDPROV_INTT.BRN_CD = {0} ) ) ";
            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = OrclDbConnection.Command(connection, _alter))
                        {
                            command.ExecuteNonQuery();
                        }
                        _statement = string.Format(_query);
                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            var parm1 = new OracleParameter("as_brn_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm1.Value = prp.brn_cd;
                            command.Parameters.Add(parm1);
                            var parm2 = new OracleParameter("asdt_dt", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm2.Value = prp.from_dt.ToString("dd/MM/yyyy");
                            command.Parameters.Add(parm2);
                            
                            command.ExecuteNonQuery();
                            transaction.Commit();
                        }
                        _statement = string.Format(_query1,
                                       string.IsNullOrWhiteSpace(prp.brn_cd) ? "brn_cd" : string.Concat("'", prp.brn_cd, "'"));
                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var tca = new tt_sbca_dtl_list();
                                        tca.acc_type_cd = UtilityM.CheckNull<int>(reader["ACC_TYPE_CD"]);
                                        tca.acc_num = UtilityM.CheckNull<string>(reader["ACC_NUM"]);
                                        tca.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                        tca.opening_dt = UtilityM.CheckNull<DateTime>(reader["OPENING_DT"]);
                                        tca.mat_dt = UtilityM.CheckNull<DateTime>(reader["MAT_DT"]);
                                        tca.constitution_cd = UtilityM.CheckNull<int>(reader["CONSTITUTION_CD"]);
                                        tca.INSTL_AMT = UtilityM.CheckNull<decimal>(reader["INSTL_AMT"]);
                                        tca.PRN_AMT = UtilityM.CheckNull<decimal>(reader["PRN_AMT"]);
                                        tca.INTT_RT = Convert.ToDecimal(UtilityM.CheckNull<float>(reader["INTT_RT"]));
                                        tca.PROV_INTT_AMT = UtilityM.CheckNull<decimal>(reader["PROV_INTT_AMT"]);
                                        tca.cust_cd = UtilityM.CheckNull<decimal>(reader["CUST_CD"]);

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

        internal List<tt_sbca_dtl_list> PopulateDLFixedDeposit(p_report_param prp)
        {
            List<tt_sbca_dtl_list> tcaRet = new List<tt_sbca_dtl_list>();
            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _query = "p_td_prov_intt_detail_list";
            string _query1 = " SELECT DISTINCT  TT_TDPROV_INTT.ACC_TYPE_CD, "
+ " TT_TDPROV_INTT.ACC_NUM,       "
+ " TT_TDPROV_INTT.CUST_NAME,     "
+ " TT_TDPROV_INTT.OPENING_DT,    "
+ " TT_TDPROV_INTT.MAT_DT,        "
+ " TT_TDPROV_INTT.PRN_AMT,       "
+ " TT_TDPROV_INTT.INTT_RT,       "
+ " TT_TDPROV_INTT.PROV_INTT_AMT, "
+ " TT_TDPROV_INTT.CONSTITUTION_CD,"
+ " TM_DEPOSIT.CUST_CD   "
+ " FROM TT_TDPROV_INTT, "
+ " TM_DEPOSIT "
+ " WHERE ( TT_TDPROV_INTT.ACC_TYPE_CD = TM_DEPOSIT.ACC_TYPE_CD ) and "
+ " ( TT_TDPROV_INTT.ACC_NUM = TM_DEPOSIT.ACC_NUM ) and "
+ " ( ( TT_TDPROV_INTT.BRN_CD = {0} ) AND  "
+ " ( TT_TDPROV_INTT.ACC_TYPE_CD = {1} ) AND   "
+ " ( trim(substr(TT_TDPROV_INTT.CONSTITUTION_CD,1,2)) = {2} ) ) ";
            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = OrclDbConnection.Command(connection, _alter))
                        {
                            command.ExecuteNonQuery();
                        }
                        _statement = string.Format(_query);
                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            var parm1 = new OracleParameter("adt_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm1.Value = prp.from_dt;
                            command.Parameters.Add(parm1);
                            var parm2 = new OracleParameter("as_brn_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm2.Value = prp.brn_cd;
                            command.Parameters.Add(parm2);

                            command.ExecuteNonQuery();
                            transaction.Commit();
                        }
                        _statement = string.Format(_query1,
                                       string.IsNullOrWhiteSpace(prp.brn_cd) ? "brn_cd" : string.Concat("'", prp.brn_cd, "'"),
                                       prp.acc_type_cd != 0 ? Convert.ToString(prp.acc_type_cd) : "ACC_TYPE_CD",
                                       prp.const_cd != 0 ? string.Concat("'", Convert.ToString(prp.const_cd), "'") : "'0'");
                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var tca = new tt_sbca_dtl_list();
                                        tca.acc_type_cd = UtilityM.CheckNull<int>(reader["ACC_TYPE_CD"]);
                                        tca.acc_num = UtilityM.CheckNull<string>(reader["ACC_NUM"]);
                                        tca.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                        tca.opening_dt = UtilityM.CheckNull<DateTime>(reader["OPENING_DT"]);
                                        var x = UtilityM.CheckNull<string>(reader["CONSTITUTION_CD"]).Substring(0,2);
                                        tca.constitution_cd = Convert.ToInt16(UtilityM.CheckNull<string>(reader["CONSTITUTION_CD"]).Substring(0,2).Trim());
                                        //tca.INSTL_AMT = UtilityM.CheckNull<decimal>(reader["INSTL_AMT"]);
                                        tca.PRN_AMT = UtilityM.CheckNull<decimal>(reader["PRN_AMT"]);
                                        tca.INTT_RT = Convert.ToDecimal(UtilityM.CheckNull<float>(reader["INTT_RT"]));
                                        tca.PROV_INTT_AMT = UtilityM.CheckNull<decimal>(reader["PROV_INTT_AMT"]);
                                        tca.cust_cd = UtilityM.CheckNull<decimal>(reader["CUST_CD"]);
                                        tca.mat_dt = UtilityM.CheckNull<DateTime>(reader["MAT_DT"]);
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

        internal List<tt_acc_stmt> PopulateASSaving(p_report_param prp)
        {
            List<tt_acc_stmt> tcaRet = new List<tt_acc_stmt>();
            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _query = "p_acc_stmt";
            string _query1 = " SELECT TT_ACC_STMT.TRANS_DT, "
+ " TT_ACC_STMT.PARTICULARS,     "
+ " TT_ACC_STMT.DR_AMT,  "
+ " TT_ACC_STMT.CR_AMT,  "
+ " TT_ACC_STMT.BALANCE, "
+ " TT_ACC_STMT.SRL_NO,  "
+ " TT_ACC_STMT.INSTRUMENT_NUM,  "
+ " ' ' c_name, "
+ " ' ' c_addr "
+ "  FROM TT_ACC_STMT "
+ " ORDER BY NVL(TT_ACC_STMT.SRL_NO,0) ASC";
            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = OrclDbConnection.Command(connection, _alter))
                        {
                            command.ExecuteNonQuery();
                        }
                        _statement = string.Format(_query);
                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            var parm1 = new OracleParameter("ad_acc_type_cd", OracleDbType.Decimal, ParameterDirection.Input);
                            parm1.Value = prp.acc_type_cd;
                            command.Parameters.Add(parm1);
                            var parm2 = new OracleParameter("as_acc_num", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm2.Value =  prp.acc_num;
                            command.Parameters.Add(parm2);
                            var parm3 = new OracleParameter("adt_from_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.from_dt;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("adt_to_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm4.Value = prp.to_dt;
                            command.Parameters.Add(parm4);

                            command.ExecuteNonQuery();
                            
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
                                        var tca = new tt_acc_stmt();
                                        tca.trans_dt = UtilityM.CheckNull<DateTime>(reader["TRANS_DT"]);
                                        tca.particulars = UtilityM.CheckNull<string>(reader["PARTICULARS"]);
                                        tca.dr_amt = UtilityM.CheckNull<decimal>(reader["DR_AMT"]);
                                        tca.cr_amt = UtilityM.CheckNull<decimal>(reader["CR_AMT"]);
                                        tca.balance = UtilityM.CheckNull<decimal>(reader["BALANCE"]);
                                         tca.srl_no = UtilityM.CheckNull<decimal>(reader["SRL_NO"]);
                                        tca.instrument_num = Convert.ToDecimal(UtilityM.CheckNull<Int64>(reader["INSTRUMENT_NUM"]));
                                        tca.c_name = UtilityM.CheckNull<string>(reader["C_NAME"]);
                                        tca.c_addr = UtilityM.CheckNull<string>(reader["C_ADDR"]);
                                        tcaRet.Add(tca);
                                    }
                                }
                            }
                        }
                        // transaction.Commit();
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

        internal List<tm_deposit> PopulateASRecuring(p_report_param prp)
        {
            List<tm_deposit> tcaRet = new List<tm_deposit>();
            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _query1 = "SELECT ACC_TYPE_CD,ACC_NUM,CUST_NAME,INSTL_DT,PAID_AMT,MONTH,YEAR,OPNG_BAL "
                +" FROM (SELECT TM_DEPOSIT.ACC_TYPE_CD,"
+ " TM_DEPOSIT.ACC_NUM,           "
+ " MM_CUSTOMER.CUST_NAME,        "
+ " TD_RD_INSTALLMENT.INSTL_DT,   "
+ " TM_DEPOSIT.INSTL_AMT PAID_AMT,"
+ " TO_CHAR(TD_RD_INSTALLMENT.DUE_DT, 'MM') MONTH,     "
+ " TO_CHAR(TD_RD_INSTALLMENT.DUE_DT, 'YYYY') YEAR,    "
+ " TM_DEPOSIT.INSTL_AMT * COUNT(B.INSTL_NUM) OPNG_BAL "
+ " FROM MM_CUSTOMER,   "
+ " TD_RD_INSTALLMENT,  "
+ " TD_RD_INSTALLMENT B,"
+ " TM_DEPOSIT          "
+ " WHERE ( TD_RD_INSTALLMENT.ACC_NUM = TM_DEPOSIT.ACC_NUM ) and "
+ " ( TD_RD_INSTALLMENT.ACC_NUM = B.ACC_NUM ) and    "
+ " ( MM_CUSTOMER.CUST_CD = TM_DEPOSIT.CUST_CD ) and "
+ " ( TD_RD_INSTALLMENT.INSTL_DT BETWEEN to_date('{0}','dd-mm-yyyy' ) AND to_date('{1}','dd-mm-yyyy' ) )  AND "
+ " ( B.INSTL_DT < to_date('{2}','dd-mm-yyyy' ) ) AND "
+ " ( B.STATUS   = 'P' ) AND "
+ " ( ( TM_DEPOSIT.ACC_NUM = {3} ) )  AND "
+ " TM_DEPOSIT.ACC_TYPE_CD = 6      "
+ " GROUP BY TM_DEPOSIT.ACC_TYPE_CD,"
+ " TM_DEPOSIT.ACC_NUM,             "
+ " MM_CUSTOMER.CUST_NAME,          "
+ " TD_RD_INSTALLMENT.INSTL_DT,     "
+ " TM_DEPOSIT.INSTL_AMT,           "
+ " TD_RD_INSTALLMENT.DUE_DT ) X ORDER BY X.INSTL_DT ASC   ";
            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = OrclDbConnection.Command(connection, _alter))
                        {
                            command.ExecuteNonQuery();
                        }
                        _statement = string.Format(_query1,
                                           prp.from_dt != null ? prp.from_dt.ToString("dd/MM/yyyy") : "from_dt",
                                           prp.to_dt != null ? prp.to_dt.ToString("dd/MM/yyyy") : "to_dt",
                                           prp.to_dt != null ? prp.to_dt.ToString("dd/MM/yyyy") : "to_dt",
                                           string.IsNullOrWhiteSpace(prp.acc_num) ? "acc_num" : string.Concat("'", prp.acc_num, "'")
                                           );

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var tca = new tm_deposit();
                                        tca.acc_type_cd = UtilityM.CheckNull<int>(reader["ACC_TYPE_CD"]);
                                        tca.acc_num = UtilityM.CheckNull<string>(reader["ACC_NUM"]);
                                        tca.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                        tca.opening_dt = UtilityM.CheckNull<DateTime>(reader["INSTL_DT"]);
                                        tca.prn_amt = UtilityM.CheckNull<decimal>(reader["PAID_AMT"]);
                                        tca.month = UtilityM.CheckNull<string>(reader["MONTH"]);
                                        tca.year = UtilityM.CheckNull<string>(reader["YEAR"]);
                                        tca.clr_bal = UtilityM.CheckNull<decimal>(reader["OPNG_BAL"]);
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

        internal List<tm_deposit> PopulateASFixedDeposit(p_report_param prp)
        {
            List<tm_deposit> tcaRet = new List<tm_deposit>();
            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _query1 = " SELECT  V_TRANS_DTLS.TRANS_DT,     "
+ " V_TRANS_DTLS.TRANS_CD,             "
+ " V_TRANS_DTLS.ACC_TYPE_CD,          "
+ " V_TRANS_DTLS.ACC_NUM,              "
+ " V_TRANS_DTLS.TRANS_MODE,           "
+ " V_TRANS_DTLS.TRF_TYPE, 	          "
+ " (SELECT DISTINCT TM_DEPOSIT.CUST_CD"
+ " FROM   TM_DEPOSIT                  "
+ " WHERE  TM_DEPOSIT.ACC_TYPE_CD = {0}          "
+ " AND    TM_DEPOSIT.ACC_NUM     = {1}              "
+ " AND    TM_DEPOSIT.RENEW_ID    = {2} ) CUST_CD,  "
+ " (SELECT TM_DEPOSIT.PRN_AMT                               "
+ " FROM   TM_DEPOSIT                                        "
+ " WHERE  TM_DEPOSIT.ACC_TYPE_CD = {3}          "
+ " AND    TM_DEPOSIT.ACC_NUM     = {4}              "
+ " AND    TM_DEPOSIT.RENEW_ID    = {5}) PRN_AMT,   "
+ " (SELECT TM_DEPOSIT.OPENING_DT                            "
+ " FROM   TM_DEPOSIT                                        "
+ " WHERE  TM_DEPOSIT.ACC_TYPE_CD = {6}          "
+ " AND    TM_DEPOSIT.ACC_NUM     = {7}              "
+ " AND    TM_DEPOSIT.RENEW_ID    =	{8}) OPENING_DT,"
+ " V_TRANS_DTLS.TRANS_TYPE,         	                     "
+ " V_TRANS_DTLS.AMOUNT                                       "
+ " FROM	  V_TRANS_DTLS                                       "
+ " WHERE   V_TRANS_DTLS.ACC_TYPE_CD = {9}        "
+ " AND   V_TRANS_DTLS.ACC_NUM     = {10}              "
+ " AND   V_TRANS_DTLS.TRANS_DT  BETWEEN to_date('{11}','dd-mm-yyyy' ) AND to_date('{12}','dd-mm-yyyy' )";
            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = OrclDbConnection.Command(connection, _alter))
                        {
                            command.ExecuteNonQuery();
                        }
                        _statement = string.Format(_query1,
                                           prp.acc_type_cd != 0 ? Convert.ToString(prp.acc_type_cd) : "ACC_TYPE_CD",
                                           string.IsNullOrWhiteSpace(prp.acc_num) ? "acc_num" : string.Concat("'", prp.acc_num, "'"),
                                           prp.const_cd != 0 ? Convert.ToString(prp.const_cd) : "0",
                                           prp.acc_type_cd != 0 ? Convert.ToString(prp.acc_type_cd) : "ACC_TYPE_CD",
                                           string.IsNullOrWhiteSpace(prp.acc_num) ? "acc_num" : string.Concat("'", prp.acc_num, "'"),
                                           prp.const_cd != 0 ? Convert.ToString(prp.const_cd) : "0",
                                           prp.acc_type_cd != 0 ? Convert.ToString(prp.acc_type_cd) : "ACC_TYPE_CD",
                                           string.IsNullOrWhiteSpace(prp.acc_num) ? "acc_num" : string.Concat("'", prp.acc_num, "'"),
                                           prp.const_cd != 0 ? Convert.ToString(prp.const_cd) : "0",
                                           prp.acc_type_cd != 0 ? Convert.ToString(prp.acc_type_cd) : "ACC_TYPE_CD",
                                           string.IsNullOrWhiteSpace(prp.acc_num) ? "acc_num" : string.Concat("'", prp.acc_num, "'"),
                                           prp.from_dt != null ? prp.from_dt.ToString("dd/MM/yyyy") : "from_dt",
                                           prp.to_dt != null ? prp.to_dt.ToString("dd/MM/yyyy") : "to_dt"
                                           );

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var tca = new tm_deposit();
                                        tca.trans_dt = UtilityM.CheckNull<DateTime>(reader["TRANS_DT"]);
                                        tca.trans_cd = Convert.ToInt64(UtilityM.CheckNull<Int64>(reader["TRANS_CD"]));
                                        tca.acc_type_cd = UtilityM.CheckNull<int>(reader["ACC_TYPE_CD"]);
                                        tca.acc_num = UtilityM.CheckNull<string>(reader["ACC_NUM"]);
                                        tca.trans_mode = UtilityM.CheckNull<string>(reader["TRANS_MODE"]);
                                        tca.intt_trf_type = UtilityM.CheckNull<string>(reader["TRF_TYPE"]);
                                        tca.cust_cd = UtilityM.CheckNull<decimal>(reader["CUST_CD"]);
                                        tca.prn_amt = UtilityM.CheckNull<decimal>(reader["PRN_AMT"]);
                                        tca.opening_dt = UtilityM.CheckNull<DateTime>(reader["OPENING_DT"]);
                                        tca.trans_type = UtilityM.CheckNull<string>(reader["TRANS_TYPE"]);
                                        var x = UtilityM.CheckNull<double>(reader["AMOUNT"]);
                                        tca.clr_bal = Convert.ToDecimal(UtilityM.CheckNull<double>(reader["AMOUNT"]));
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

        internal List<tt_opn_cls_register> PopulateOpenCloseRegister(p_report_param prp)
        {
            List<tt_opn_cls_register> tcaRet = new List<tt_opn_cls_register>();
            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _query = "P_opn_cls_register";
            string _query1 = " SELECT TT_OPN_CLS_REGISTER.ACC_TYPE_CD,"
+ " TT_OPN_CLS_REGISTER.ACC_NUM,           "
+ " TT_OPN_CLS_REGISTER.CUST_CD,           "
+ " TT_OPN_CLS_REGISTER.OPN_CLS_DT,        "
+ " TT_OPN_CLS_REGISTER.PRN_AMT,           "
+ " TT_OPN_CLS_REGISTER.INTT_AMT,          "
+ " TT_OPN_CLS_REGISTER.PENAL_AMT,         "
+ " TT_OPN_CLS_REGISTER.STATUS,            "
+ " TT_OPN_CLS_REGISTER.INTT_RT,           "
+ " TT_OPN_CLS_REGISTER.DEP_PERIOD,        "
+ " TT_OPN_CLS_REGISTER.AGENT_CD,           "
+ "  MM_CUSTOMER.CUST_NAME "
+ " FROM TT_OPN_CLS_REGISTER   ,MM_CUSTOMER "
+ " WHERE TT_OPN_CLS_REGISTER.CUST_CD =MM_CUSTOMER.CUST_CD ";
            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
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
                            var parm3 = new OracleParameter("as_brn_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm3.Value = prp.brn_cd;
                            command.Parameters.Add(parm3);

                            var parm4 = new OracleParameter("as_flag", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm4.Value = prp.flag;
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
                                        var tca = new tt_opn_cls_register();
                                        tca.acc_type_cd = UtilityM.CheckNull<Int64>(reader["ACC_TYPE_CD"]);
                                        tca.acc_num = UtilityM.CheckNull<string>(reader["ACC_NUM"]);
                                        tca.cust_cd = UtilityM.CheckNull<Int64>(reader["CUST_CD"]);
                                        tca.opn_cls_dt = UtilityM.CheckNull<DateTime>(reader["OPN_CLS_DT"]);
                                        tca.prn_amt = UtilityM.CheckNull<decimal>(reader["PRN_AMT"]);
                                        tca.intt_amt = UtilityM.CheckNull<decimal>(reader["INTT_AMT"]);
                                        tca.penal_amt = UtilityM.CheckNull<decimal>(reader["PENAL_AMT"]);
                                        tca.status = UtilityM.CheckNull<string>(reader["STATUS"]);
                                        tca.intt_rt = UtilityM.CheckNull<decimal>(reader["INTT_RT"]);
                                        tca.dep_period = UtilityM.CheckNull<string>(reader["DEP_PERIOD"]);
                                        tca.agent_cd = UtilityM.CheckNull<string>(reader["AGENT_CD"]);
                                        tca.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
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

        internal List<tm_deposit> PopulateNearMatDetails(p_report_param prp)
        {
            List<tm_deposit> tcaRet = new List<tm_deposit>();
            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _query1 = " SELECT MM_ACC_TYPE.ACC_TYPE_DESC,       "
+ " TM_DEPOSIT.ACC_NUM,                     "
+ " MM_CUSTOMER.CUST_NAME,                  "
+ " TM_DEPOSIT.OPENING_DT,                  "
+ " TM_DEPOSIT.MAT_DT,                      "
+ " TM_DEPOSIT.PRN_AMT,                     "
+ " NVL(TM_DEPOSIT.LOCK_MODE,'U') LOCK_MODE, "
+ " NVL(TM_DEPOSIT.INSTL_AMT,1) INSTL_AMT,  "
+ " NVL(TM_DEPOSIT.INSTL_NO,1) INSTL_NO,    "
+ " TM_DEPOSIT.INTT_RT,                     "
+ " TM_DEPOSIT.INTT_TRF_TYPE,               "
+ " TM_DEPOSIT.ACC_TYPE_CD,                 "
+ " DECODE(TM_DEPOSIT.ACC_TYPE_CD , 6, f_calcrdintt_reg(TM_DEPOSIT.ACC_NUM, "
+ " TM_DEPOSIT.INSTL_AMT,    "
+ " TM_DEPOSIT.INSTL_NO,     "
+ " TM_DEPOSIT.INTT_RT),     "
+ " f_calctdintt_reg(TM_DEPOSIT.ACC_TYPE_CD,  "
+ " TM_DEPOSIT.PRN_AMT,       "
+ " TM_DEPOSIT.OPENING_DT,    "
+ " TM_DEPOSIT.INTT_TRF_TYPE, "
+ " TM_DEPOSIT.MAT_DT - TM_DEPOSIT.OPENING_DT, "
+ " TM_DEPOSIT.INTT_RT)) INTT"
+ " FROM TM_DEPOSIT,         "
+ " MM_ACC_TYPE,             "
+ " MM_CUSTOMER              "
+ " WHERE TM_DEPOSIT.BRN_CD = {0} and             "
+ " TM_DEPOSIT.ACC_TYPE_CD = MM_ACC_TYPE.ACC_TYPE_CD and "
+ " TM_DEPOSIT.CUST_CD = MM_CUSTOMER.CUST_CD and         "
+ " NVL(TM_DEPOSIT.ACC_STATUS,'O') <> 'C' and            "
+ " (TM_DEPOSIT.RENEW_ID <> 1  and                       "
+ " TM_DEPOSIT.RENEW_ID = 0 ) and                        "
+ " (TM_DEPOSIT.MAT_DT BETWEEN to_date('{1}','dd-mm-yyyy' ) AND to_date('{2}','dd-mm-yyyy' ) ) and "
+ " TM_DEPOSIT.ACC_TYPE_CD not in ( 1, 7 ) ";

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = OrclDbConnection.Command(connection, _alter))
                        {
                            command.ExecuteNonQuery();
                        }
                        _statement = string.Format(_query1,
                                           string.IsNullOrWhiteSpace(prp.brn_cd) ? "brn_cd" : string.Concat("'", prp.brn_cd, "'"),
                                           prp.from_dt != null ? prp.from_dt.ToString("dd/MM/yyyy") : "from_dt",
                                           prp.to_dt != null ? prp.to_dt.ToString("dd/MM/yyyy") : "to_dt"
                                           );

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var tca = new tm_deposit();
                                        tca.acc_type_desc = UtilityM.CheckNull<string>(reader["ACC_TYPE_DESC"]);
                                        tca.acc_num = UtilityM.CheckNull<string>(reader["ACC_NUM"]);
                                        tca.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                        tca.opening_dt = UtilityM.CheckNull<DateTime>(reader["OPENING_DT"]);
                                        tca.mat_dt = UtilityM.CheckNull<DateTime>(reader["MAT_DT"]);
                                        tca.prn_amt = UtilityM.CheckNull<decimal>(reader["PRN_AMT"]);
                                        tca.lock_mode = UtilityM.CheckNull<string>(reader["LOCK_MODE"]);
                                        tca.instl_amt = UtilityM.CheckNull<decimal>(reader["INSTL_AMT"]);
                                        tca.instl_no = UtilityM.CheckNull<decimal>(reader["INSTL_NO"]);
                                        tca.intt_rt = UtilityM.CheckNull<decimal>(reader["INTT_RT"]);
                                        tca.intt_trf_type = UtilityM.CheckNull<string>(reader["INTT_TRF_TYPE"]);
                                        tca.acc_type_cd = UtilityM.CheckNull<int>(reader["ACC_TYPE_CD"]);
                                        tca.intt_amt = UtilityM.CheckNull<decimal>(reader["INTT"]);
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

        internal List<tm_deposit> GetDepositWithChild(tm_deposit dep)
        {
            List<tm_deposit> depoList = new List<tm_deposit>();

            string _query = " SELECT TD.BRN_CD BRN_CD, TD.ACC_TYPE_CD ACC_TYPE_CD, TD.ACC_NUM ACC_NUM, TD.RENEW_ID RENEW_ID, TD.CUST_CD CUST_CD, MC.CUST_NAME CUST_NAME,MC.PERMANENT_ADDRESS PERMANENT_ADDRESS,MC.PHONE PHONE,MC.GUARDIAN_NAME GUARDIAN_NAME,MC.DT_OF_BIRTH DT_OF_BIRTH,MC.SEX SEX,MC.OCCUPATION OCCUPATION,MC.PRESENT_ADDRESS PRESENT_ADDRESS "
                            + " FROM TM_DEPOSIT TD,  MM_CUSTOMER MC,  MM_CONSTITUTION MCO   "
                            + " WHERE TD.BRN_CD={0} AND TD.ACC_NUM={1} AND TD.ACC_TYPE_CD={2} AND TD.CUST_CD=MC.CUST_CD AND TD.CONSTITUTION_CD =MCO.CONSTITUTION_CD AND TD.ACC_TYPE_CD=MCO.ACC_TYPE_CD ";

            using (var connection = OrclDbConnection.NewConnection)
            {
                _statement = string.Format(_query,
                                          !string.IsNullOrWhiteSpace(dep.brn_cd) ? string.Concat("'", dep.brn_cd, "'") : "brn_cd",
                                          !string.IsNullOrWhiteSpace(dep.acc_num) ? string.Concat("'", dep.acc_num, "'") : "acc_num",
                                          dep.acc_type_cd != 0 ? Convert.ToString(dep.acc_type_cd) : "ACC_TYPE_CD"
                                           );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        try
                        {
                            if (reader.HasRows)
                            {
                                {
                                    while (reader.Read())
                                    {
                                        var d = new tm_deposit();
                                        d.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                        d.acc_type_cd = UtilityM.CheckNull<int>(reader["ACC_TYPE_CD"]);
                                        d.acc_num = UtilityM.CheckNull<string>(reader["ACC_NUM"]);
                                        d.renew_id = UtilityM.CheckNull<int>(reader["RENEW_ID"]);
                                        d.cust_cd = UtilityM.CheckNull<decimal>(reader["CUST_CD"]);
                                        d.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                        d.guardian_name = UtilityM.CheckNull<string>(reader["GUARDIAN_NAME"]);
                                        d.dt_of_birth = UtilityM.CheckNull<DateTime>(reader["DT_OF_BIRTH"]);
                                        d.sex = UtilityM.CheckNull<string>(reader["SEX"]);
                                        d.permanent_address = UtilityM.CheckNull<string>(reader["PERMANENT_ADDRESS"]);
                                        d.occupation = UtilityM.CheckNull<string>(reader["OCCUPATION"]);
                                        d.phone = UtilityM.CheckNull<string>(reader["PHONE"]);
                                        d.present_address = UtilityM.CheckNull<string>(reader["PRESENT_ADDRESS"]);
                                        depoList.Add(d);
                                    }
                                }
                            }

                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }

            return depoList;
        }

        internal List<td_outward_payment> GetNeftOutDtls(p_report_param prp)
        {
            List<td_outward_payment> nomList = new List<td_outward_payment>();

            string _query = " SELECT TD_OUTWARD_PAYMENT.BRN_CD, "
    + " TD_OUTWARD_PAYMENT.TRANS_DT, "
    + " TD_OUTWARD_PAYMENT.TRANS_CD, "
    + " TD_OUTWARD_PAYMENT.PAYMENT_TYPE, "
    + " TD_OUTWARD_PAYMENT.BENE_NAME, "
    + " TD_OUTWARD_PAYMENT.BENE_CODE, "
    + " TD_OUTWARD_PAYMENT.AMOUNT, "
    + " TD_OUTWARD_PAYMENT.CHARGE_DED, "
    + " TD_OUTWARD_PAYMENT.DATE_OF_PAYMENT, "
    + " TD_OUTWARD_PAYMENT.BENE_ACC_NO, "
    + " TD_OUTWARD_PAYMENT.BENE_IFSC_CODE, "
    + " TD_OUTWARD_PAYMENT.DR_ACC_NO, "
    + " TD_OUTWARD_PAYMENT.BENE_EMAIL_ID, "
    + " TD_OUTWARD_PAYMENT.BENE_MOBILE_NO, "
    + " TD_OUTWARD_PAYMENT.BANK_DR_ACC_TYPE, "
    + " TD_OUTWARD_PAYMENT.BANK_DR_ACC_NO, "
    + " TD_OUTWARD_PAYMENT.BANK_DR_ACC_NAME, "
    + " TD_OUTWARD_PAYMENT.CREDIT_NARRATION, "
    + " TD_OUTWARD_PAYMENT.PAYMENT_REF_NO, "
    + " TD_OUTWARD_PAYMENT.STATUS, "
    + " TD_OUTWARD_PAYMENT.REJECTION_REASON, "
    + " TD_OUTWARD_PAYMENT.PROCESSING_REMARKS, "
    + " TD_OUTWARD_PAYMENT.CUST_REF_NO, "
    + " TD_OUTWARD_PAYMENT.VALUE_DATE, "
    + " TD_OUTWARD_PAYMENT.CREATED_BY, "
    + " TD_OUTWARD_PAYMENT.CREATED_DT, "
    + " TD_OUTWARD_PAYMENT.MODIFIED_BY, "
    + " TD_OUTWARD_PAYMENT.MODIFIED_DT, "
    + " TD_OUTWARD_PAYMENT.APPROVED_BY, "
    + " TD_OUTWARD_PAYMENT.APPROVED_DT, "
    + " TD_OUTWARD_PAYMENT.APPROVAL_STATUS "
    + " FROM TD_OUTWARD_PAYMENT "
    + " WHERE ( TD_OUTWARD_PAYMENT.BRN_CD = {0} ) AND "
    + " ( TD_OUTWARD_PAYMENT.TRANS_DT  BETWEEN to_date('{1}','dd-mm-yyyy' ) AND to_date('{2}','dd-mm-yyyy' ) )";

            using (var connection = OrclDbConnection.NewConnection)
            {
                _statement = string.Format(_query,
                                           string.IsNullOrWhiteSpace(prp.brn_cd) ? "brn_cd" : string.Concat("'", prp.brn_cd, "'"),
                                           prp.from_dt != null ? prp.from_dt.ToString("dd/MM/yyyy") : "from_dt",
                                           prp.to_dt != null ? prp.to_dt.ToString("dd/MM/yyyy") : "to_dt"
                                           );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            {
                                while (reader.Read())
                                {
                                    var n = new td_outward_payment();
                                    n.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                    n.trans_dt = UtilityM.CheckNull<DateTime>(reader["TRANS_DT"]);
                                    n.trans_cd = UtilityM.CheckNull<Int64>(reader["TRANS_CD"]);
                                    n.payment_type = UtilityM.CheckNull<string>(reader["PAYMENT_TYPE"]);
                                    n.bene_name = UtilityM.CheckNull<string>(reader["BENE_NAME"]);
                                    n.bene_code = UtilityM.CheckNull<string>(reader["BENE_CODE"]);
                                    n.amount = UtilityM.CheckNull<decimal>(reader["AMOUNT"]);
                                    n.charge_ded = UtilityM.CheckNull<double>(reader["CHARGE_DED"]);
                                    n.date_of_payment = UtilityM.CheckNull<DateTime>(reader["DATE_OF_PAYMENT"]);
                                    n.bene_acc_no = UtilityM.CheckNull<string>(reader["BENE_ACC_NO"]);
                                    n.bene_ifsc_code = UtilityM.CheckNull<string>(reader["BENE_IFSC_CODE"]);
                                    n.dr_acc_no = UtilityM.CheckNull<Int64>(reader["DR_ACC_NO"]);
                                    n.bene_email_id = UtilityM.CheckNull<string>(reader["BENE_EMAIL_ID"]);
                                    n.bene_mobile_no = UtilityM.CheckNull<Int64>(reader["BENE_MOBILE_NO"]);
                                    n.bank_dr_acc_type = UtilityM.CheckNull<Int32>(reader["BANK_DR_ACC_TYPE"]);
                                    n.bank_dr_acc_no = UtilityM.CheckNull<string>(reader["BANK_DR_ACC_NO"]);
                                    n.bank_dr_acc_name = UtilityM.CheckNull<string>(reader["BANK_DR_ACC_NAME"]);
                                    n.credit_narration = UtilityM.CheckNull<string>(reader["CREDIT_NARRATION"]);
                                    n.payment_ref_no = UtilityM.CheckNull<string>(reader["PAYMENT_REF_NO"]);
                                    n.status = UtilityM.CheckNull<string>(reader["STATUS"]);
                                    n.rejection_reason = UtilityM.CheckNull<string>(reader["REJECTION_REASON"]);
                                    n.processing_remarks = UtilityM.CheckNull<string>(reader["PROCESSING_REMARKS"]);
                                    n.cust_ref_no = UtilityM.CheckNull<string>(reader["CUST_REF_NO"]);
                                    n.value_date = UtilityM.CheckNull<DateTime>(reader["VALUE_DATE"]);
                                    n.created_by = UtilityM.CheckNull<string>(reader["CREATED_BY"]);
                                    n.created_dt = UtilityM.CheckNull<DateTime>(reader["CREATED_DT"]);
                                    n.modified_by = UtilityM.CheckNull<string>(reader["MODIFIED_BY"]);
                                    n.modified_dt = UtilityM.CheckNull<DateTime>(reader["MODIFIED_DT"]);
                                    n.approved_by = UtilityM.CheckNull<string>(reader["APPROVED_BY"]);
                                    n.approved_dt = UtilityM.CheckNull<DateTime>(reader["APPROVED_DT"]);
                                    n.approval_status = UtilityM.CheckNull<string>(reader["APPROVAL_STATUS"]);

                                    nomList.Add(n);
                                }
                            }
                        }
                    }
                }
            }

            return nomList;
        }



        internal List<neft_inward> NeftInward(p_report_param prp)
        {
            List<neft_inward> neftInward = new List<neft_inward>();
            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";

            string _query = " SELECT TD_INWARD_RECEIVE.BRN_CD, "
                   + " TD_INWARD_RECEIVE.TRANS_DT,             "
                   + " TD_INWARD_RECEIVE.TRANS_CD,             "
                   + " TD_INWARD_RECEIVE.RECEIVE_TYPE,         "
                   + " TD_INWARD_RECEIVE.BANK_CR_ACC_NO,       "
                   + " TD_INWARD_RECEIVE.AMOUNT,               "
                   + " TD_INWARD_RECEIVE.DATE_OF_RECEIVE,      "
                   + " TD_INWARD_RECEIVE.PAYMENT_REF_NO,       "
                   + " TD_INWARD_RECEIVE.SENDER_ACC_NO,        "
                   + " TD_INWARD_RECEIVE.SENDER_IFSC_CODE,     "
                   + " TD_INWARD_RECEIVE.SENDER_NAME,          "
                   + " TD_INWARD_RECEIVE.CR_ACC_NO,            "
                   + " MM_CUSTOMER.CUST_NAME BANK_CR_ACC_NAME, "
                   + " TD_INWARD_RECEIVE.BANK_NAME,            "
                   + " TD_INWARD_RECEIVE.STATUS,               "
                   + " TD_INWARD_RECEIVE.REJECTION_REASON,     "
                   + " TD_INWARD_RECEIVE.CUST_REF_NO,          "
                   + " TD_INWARD_RECEIVE.VALUE_DATE            "
                   + " FROM TD_INWARD_RECEIVE, TM_DEPOSIT, MM_CUSTOMER     "
                   + " WHERE TD_INWARD_RECEIVE.BANK_CR_ACC_NO = TM_DEPOSIT.ACC_NUM     "
                   + " AND TM_DEPOSIT.CUST_CD = MM_CUSTOMER.CUST_CD                    "
                   + " AND TM_DEPOSIT.ACC_TYPE_CD = 1                                  "
                   + " And TD_INWARD_RECEIVE.BRN_CD = {0}                              "
                   + " AND TD_INWARD_RECEIVE.TRANS_DT BETWEEN to_date('{1}', 'dd-mm-yyyy') AND to_date('{2}', 'dd-mm-yyyy') ";

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = OrclDbConnection.Command(connection, _alter))
                        {
                            command.ExecuteNonQuery();
                        }

                        _statement = string.Format(_query,
                                       string.IsNullOrWhiteSpace(prp.brn_cd) ? "brn_cd" : string.Concat("'", prp.brn_cd, "'"),
                                       prp.from_dt != null ? prp.from_dt.ToString("dd/MM/yyyy") : "from_dt",
                                       prp.to_dt != null ? prp.to_dt.ToString("dd/MM/yyyy") : "to_dt"
                                       );

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var neft = new neft_inward();

                                        neft.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                        neft.trans_dt = UtilityM.CheckNull<DateTime>(reader["TRANS_DT"]);
                                        neft.trans_cd = UtilityM.CheckNull<Int64>(reader["TRANS_CD"]);
                                        neft.receive_type = UtilityM.CheckNull<string>(reader["RECEIVE_TYPE"]);
                                        neft.bank_cr_acc_no = UtilityM.CheckNull<string>(reader["BANK_CR_ACC_NO"]);
                                        neft.amount = UtilityM.CheckNull<decimal>(reader["AMOUNT"]);
                                        neft.date_of_receive = UtilityM.CheckNull<DateTime>(reader["DATE_OF_RECEIVE"]);
                                        neft.payment_ref_no = UtilityM.CheckNull<string>(reader["PAYMENT_REF_NO"]);
                                        neft.sender_acc_no = UtilityM.CheckNull<string>(reader["SENDER_ACC_NO"]);
                                        neft.sender_ifsc_code = UtilityM.CheckNull<string>(reader["SENDER_IFSC_CODE"]);
                                        neft.sender_name = UtilityM.CheckNull<string>(reader["SENDER_NAME"]);
                                        neft.cr_acc_no = UtilityM.CheckNull<Int64>(reader["CR_ACC_NO"]);
                                        neft.bank_cr_acc_name = UtilityM.CheckNull<string>(reader["BANK_CR_ACC_NAME"]);
                                        neft.bank_name = UtilityM.CheckNull<string>(reader["BANK_NAME"]);
                                        neft.status = UtilityM.CheckNull<string>(reader["STATUS"]);
                                        neft.rejection_reason = UtilityM.CheckNull<string>(reader["REJECTION_REASON"]);
                                        neft.cust_ref_no = UtilityM.CheckNull<string>(reader["CUST_REF_NO"]);
                                        neft.value_date = UtilityM.CheckNull<DateTime>(reader["VALUE_DATE"]);

                                        neftInward.Add(neft);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        neftInward = null;
                    }
                }
            }

            return neftInward;
        }

        internal List<passbook_print> PassBookPrint(p_report_param prp)
        {
            List<passbook_print> passBookPrint = new List<passbook_print>();
            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";

            // prp.from_dt = prp.from_dt.Date;
            // prp.to_dt = prp.to_dt.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            string _query = " SELECT MD_PASSBOOK_PRINT_STATUS.TRANS_DT,         "
                  + " MD_PASSBOOK_PRINT_STATUS.TRANS_CD,                        "
                  + " MD_PASSBOOK_PRINT_STATUS.ACC_TYPE_CD,                     "
                  + " MD_PASSBOOK_PRINT_STATUS.ACC_NUM,                         "
                  + " MD_PASSBOOK_PRINT_STATUS.TRANS_TYPE,                      "
                  + " MD_PASSBOOK_PRINT_STATUS.INSTRUMENT_NUM,                  "
                  + " MD_PASSBOOK_PRINT_STATUS.AMOUNT,                          "
                  + " Lower(MD_PASSBOOK_PRINT_STATUS.PARTICULARS) PARTICULARS,  "
                  + " MD_PASSBOOK_PRINT_STATUS.PRINTED_FLAG,                    "
                  + " f_passbook_balance({0}, {1}, to_date('{2}', 'dd-mm-yyyy hh24:mi:ss') ) BALANCE_AMT,            "
                  + " 0 RUNNING_BAL,                                                             "
                  + " 0 ROWCD                                                                    "
                  + " FROM MD_PASSBOOK_PRINT_STATUS                                              "
                  + " WHERE ((MD_PASSBOOK_PRINT_STATUS.ACC_TYPE_CD = {3} ) AND                   "
                  + "       (MD_PASSBOOK_PRINT_STATUS.ACC_NUM = '{4}' ))                         "
                  + " AND   (MD_PASSBOOK_PRINT_STATUS.PRINTED_FLAG = 'N')                        "
                  + " AND   (MD_PASSBOOK_PRINT_STATUS.TRANS_DT BETWEEN to_date('{5}', 'dd-mm-yyyy hh24:mi:ss') AND to_date('{6}', 'dd-mm-yyyy hh24:mi:ss') ) "
                  + " ORDER BY MD_PASSBOOK_PRINT_STATUS.TRANS_DT, MD_PASSBOOK_PRINT_STATUS.TRANS_CD ";

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = OrclDbConnection.Command(connection, _alter))
                        {
                            command.ExecuteNonQuery();
                        }

                        _statement = string.Format(_query,
                                                   prp.acc_type_cd.ToString() ,
                                                   prp.acc_num,
                                                   prp.to_dt.ToString("dd/MM/yyyy HH:mm:ss"),
                                                   prp.acc_type_cd.ToString(),
                                                   prp.acc_num,
                                                   prp.from_dt != null ? prp.from_dt.ToString("dd/MM/yyyy HH:mm:ss") : "from_dt",
                                                   prp.to_dt   != null ? prp.to_dt.ToString("dd/MM/yyyy HH:mm:ss")   : "to_dt"
                                                   );

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var pb = new passbook_print();

                                        pb.trans_dt = UtilityM.CheckNull<DateTime>(reader["TRANS_DT"]);
                                        pb.trans_cd = UtilityM.CheckNull<decimal>(reader["TRANS_CD"]);
                                        // pb.acc_type_cd = UtilityM.CheckNull<Int16>(reader["ACC_TYPE_CD"]);
                                        pb.acc_num = UtilityM.CheckNull<string>(reader["ACC_NUM"]);
                                        pb.trans_type = UtilityM.CheckNull<string>(reader["TRANS_TYPE"]);
                                        pb.amount = UtilityM.CheckNull<decimal>(reader["AMOUNT"]);
                                        pb.particulars = UtilityM.CheckNull<string>(reader["PARTICULARS"]);
                                        pb.printed_flag = UtilityM.CheckNull<string>(reader["PRINTED_FLAG"]);
                                        pb.balance_amt = UtilityM.CheckNull<decimal>(reader["BALANCE_AMT"]);
                                        pb.running_bal = UtilityM.CheckNull<decimal>(reader["RUNNING_BAL"]);
                                        pb.rowcd = UtilityM.CheckNull<decimal>(reader["ROWCD"]);

                                        passBookPrint.Add(pb);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        passBookPrint = null;
                    }
                }
            }

            return passBookPrint;
        }


        internal List<active_standing_instr> GetActiveStandingInstr(p_report_param prp)
        {
            List<active_standing_instr> standingInstrList = new List<active_standing_instr>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _query = " SELECT A.ACC_TYPE_FROM ACC_TYPE_FROM, A.ACC_NUM_FROM  ACC_NUM_FROM, "
                            + " B.ACC_TYPE_TO   ACC_TYPE_TO,   B.ACC_NUM_TO    ACC_NUM_TO,          "
                            + " A.INSTR_STATUS  INSTR_STATUS,  A.FIRST_TRF_DT  FIRST_TRF_DT,        "
                            + " A.PERIODICITY   PERIODICITY,   A.PRN_INTT_FLAG PRN_INTT_FLAG,       "
                            + " A.AMOUNT        AMOUNT,        A.SRL_NO        SRL_NO               "
                            + " FROM SM_STANDING_INSTRUCTION A, SD_STANDING_INSTRUCTION B           "
                            + " WHERE NVL(A.INSTR_STATUS, 'O') = 'O' AND A.SRL_NO = B.SRL_NO        "
                            + " AND A.BRN_CD = B.BRN_CD                                             "
                            + " AND A.BRN_CD = '{0}'                                                  "
                            + " ORDER BY A.ACC_TYPE_FROM, A.ACC_NUM_FROM                            ";

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = OrclDbConnection.Command(connection, _alter))
                        {
                            command.ExecuteNonQuery();
                        }

                        _statement = string.Format(_query,
                                                   prp.brn_cd);

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var stInstr = new active_standing_instr();

                                        stInstr.acc_type_from = UtilityM.CheckNull<int>(reader["ACC_TYPE_FROM"]);
                                        stInstr.acc_num_from = UtilityM.CheckNull<string>(reader["ACC_NUM_FROM"]);
                                        stInstr.acc_type_to = UtilityM.CheckNull<int>(reader["ACC_TYPE_TO"]);
                                        stInstr.acc_num_to = UtilityM.CheckNull<string>(reader["ACC_NUM_TO"]);
                                        stInstr.instr_status = UtilityM.CheckNull<string>(reader["INSTR_STATUS"]);
                                        stInstr.first_trf_dt = UtilityM.CheckNull<DateTime>(reader["FIRST_TRF_DT"]);
                                        stInstr.periodicity = UtilityM.CheckNull<string>(reader["PERIODICITY"]);
                                        stInstr.prn_intt_flag = UtilityM.CheckNull<string>(reader["PRN_INTT_FLAG"]);
                                        stInstr.amount = UtilityM.CheckNull<decimal>(reader["AMOUNT"]);
                                        stInstr.srl_no = UtilityM.CheckNull<decimal>(reader["SRL_NO"]);

                                        standingInstrList.Add(stInstr);
                                    }
                                    transaction.Commit();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        standingInstrList = null;
                    }
                }
            }
            return standingInstrList;
        }


        internal List<standing_instr_exe> GetStandingInstrExe(p_report_param prp)
        {
            List<standing_instr_exe> standingInstrExeList = new List<standing_instr_exe>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "p_si_rep";
            string _query = " SELECT TT_EXECUTED_SI.TRANS_DT,   "
                             + " TT_EXECUTED_SI.DR_ACC_TYPE,     "
                             + " dr.ACC_TYPE_DESC as DR_ACC_TYPE_DESC, "
                             + " cr.ACC_TYPE_DESC as CR_ACC_TYPE_DESC, "
                             + " TT_EXECUTED_SI.DR_ACC_NUM,      "
                             + " TT_EXECUTED_SI.CUST_CD,         "
                             + " c.CUST_NAME,                    "
                             + " TT_EXECUTED_SI.CR_ACC_TYPE,     "
                             + " TT_EXECUTED_SI.CR_ACC_NUM,      "
                             + " TT_EXECUTED_SI.AMOUNT           "
                             + " FROM TT_EXECUTED_SI , MM_ACC_TYPE   dr , MM_ACC_TYPE cr  , MM_CUSTOMER c "
                             + " WHERE TT_EXECUTED_SI.TRANS_DT = to_date('{0}', 'dd-mm-yyyy') "
                             + "  AND c.CUST_CD = TT_EXECUTED_SI.CUST_CD "
                             + "  AND dr.ACC_TYPE_CD = DR_ACC_TYPE "
                             + "  AND cr.ACC_TYPE_CD = CR_ACC_TYPE ";

           // prp.adt_dt = prp.adt_dt.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            using (var connection = OrclDbConnection.NewConnection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = OrclDbConnection.Command(connection, _alter))
                        {
                            command.ExecuteNonQuery();
                        }

                        _statement = string.Format(_procedure);
                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;

                            var parm1 = new OracleParameter("adt_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm1.Value = prp.adt_dt;
                            command.Parameters.Add(parm1);

                            var parm2 = new OracleParameter("as_brn_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm2.Value = prp.brn_cd;
                            command.Parameters.Add(parm2);

                            command.ExecuteNonQuery();
                        }


                        _statement = string.Format(_query,
                                                    prp.adt_dt.ToString("dd/MM/yyyy"));

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var stInstrExe = new standing_instr_exe();

                                        stInstrExe.trans_dt = UtilityM.CheckNull<DateTime>(reader["TRANS_DT"]);
                                        // stInstrExe.dr_acc_type = UtilityM.CheckNull<Int32>(reader["DR_ACC_TYPE"]);
                                        stInstrExe.dr_acc_type_desc = UtilityM.CheckNull<string>(reader["DR_ACC_TYPE_DESC"]);
                                        stInstrExe.dr_acc_num = UtilityM.CheckNull<string>(reader["DR_ACC_NUM"]);
                                        // stInstrExe.cust_cd = UtilityM.CheckNull<Int32>(reader["CUST_CD"]);
                                        stInstrExe.cust_name = UtilityM.CheckNull<string>(reader["CUST_name"]);
                                        // stInstrExe.cr_acc_type = UtilityM.CheckNull<Int32>(reader["CR_ACC_TYPE"]);
                                        stInstrExe.cr_acc_type_desc = UtilityM.CheckNull<string>(reader["CR_ACC_TYPE_DESC"]);
                                        stInstrExe.cr_acc_num = UtilityM.CheckNull<string>(reader["CR_ACC_NUM"]);
                                        stInstrExe.amount = UtilityM.CheckNull<decimal>(reader["AMOUNT"]);

                                        standingInstrExeList.Add(stInstrExe);
                                    }
                                    transaction.Commit();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        standingInstrExeList = null;
                    }
                }
            }
            return standingInstrExeList;
        }



    }
}