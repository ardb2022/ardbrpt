using Oracle.DataAccess.Client;
using RDLCReportServer.Model;
using RDLCReportServer.Util;
using SBWSFinanceApi.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace RDLCReportServer.DL.Loan
{
    public class LoanDL
    {

        internal List<tt_detailed_list_loan> PopulateLoanDetailedList(p_report_param prp)
        {
            List<tt_detailed_list_loan> loanDtlList = new List<tt_detailed_list_loan>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "p_detailed_list_loan";
            string _query = "   SELECT TT_DETAILED_LIST_LOAN.ACC_CD,           "
                             + "   TT_DETAILED_LIST_LOAN.PARTY_NAME,           "
                             + "   TT_DETAILED_LIST_LOAN.CURR_INTT_RATE,       "
                             + "   TT_DETAILED_LIST_LOAN.OVD_INTT_RATE,        "
                             + "   TT_DETAILED_LIST_LOAN.CURR_PRN,             "
                             + "   TT_DETAILED_LIST_LOAN.OVD_PRN,              "
                             + "   TT_DETAILED_LIST_LOAN.CURR_INTT,            "
                             + "   TT_DETAILED_LIST_LOAN.OVD_INTT,             "
                             + "   TT_DETAILED_LIST_LOAN.ACC_NAME,             "
                             + "   TT_DETAILED_LIST_LOAN.ACC_NUM,              "
                             + "   TT_DETAILED_LIST_LOAN.BLOCK_NAME,           "
                             + "   TT_DETAILED_LIST_LOAN.COMPUTED_TILL_DT,     "
                             + "   TT_DETAILED_LIST_LOAN.LIST_DT               "
                             + "   FROM  TT_DETAILED_LIST_LOAN                 "
                             + "   WHERE TT_DETAILED_LIST_LOAN.ACC_CD = {0}    "
                             + "   AND  ( TT_DETAILED_LIST_LOAN.CURR_PRN + TT_DETAILED_LIST_LOAN.OVD_PRN ) > 0 ";



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

                            var parm1 = new OracleParameter("as_brn_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm1.Value = prp.brn_cd;
                            command.Parameters.Add(parm1);

                            var parm2 = new OracleParameter("ad_acc_cd", OracleDbType.Int32, ParameterDirection.Input);
                            parm2.Value = prp.acc_cd;
                            command.Parameters.Add(parm2);

                            var parm3 = new OracleParameter("adt_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.adt_dt;
                            command.Parameters.Add(parm3);

                            command.ExecuteNonQuery();

                        }

                        _statement = string.Format(_query,
                                                   prp.acc_cd != 0 ? Convert.ToString(prp.acc_cd) : "ACC_CD");

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanDtl = new tt_detailed_list_loan();

                                        loanDtl.acc_cd = Convert.ToInt32(UtilityM.CheckNull<Int32>(reader["ACC_CD"]));
                                        loanDtl.party_name = UtilityM.CheckNull<string>(reader["PARTY_NAME"]);
                                        loanDtl.curr_intt_rate = UtilityM.CheckNull<float>(reader["CURR_INTT_RATE"]);
                                        loanDtl.ovd_intt_rate = UtilityM.CheckNull<float>(reader["OVD_INTT_RATE"]);
                                        loanDtl.curr_prn = UtilityM.CheckNull<decimal>(reader["CURR_PRN"]);
                                        loanDtl.ovd_prn = UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                        loanDtl.curr_intt = UtilityM.CheckNull<decimal>(reader["CURR_INTT"]);
                                        loanDtl.ovd_intt = UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);
                                        loanDtl.acc_name = UtilityM.CheckNull<string>(reader["ACC_NAME"]);
                                        loanDtl.acc_num = UtilityM.CheckNull<string>(reader["ACC_NUM"]);
                                        loanDtl.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                        loanDtl.computed_till_dt = UtilityM.CheckNull<DateTime>(reader["COMPUTED_TILL_DT"]);
                                        loanDtl.list_dt = UtilityM.CheckNull<DateTime>(reader["LIST_DT"]);
                                        loanDtlList.Add(loanDtl);
                                    }
                                    transaction.Commit();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanDtlList = null;
                    }
                }
            }
            return loanDtlList;
        }




        internal List<tm_loan_all> PopulateLoanDisburseReg(p_report_param prp)
        {
            List<tm_loan_all> loanDisReg = new List<tm_loan_all>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _query = " SELECT A.LOAN_ID,   "
                            + " A.ACC_CD,         "
                            + " B.TRANS_DT,       "
                            + " A.PARTY_CD,       "
                            + " C.CUST_NAME,       "
                            + " A.BRN_CD,         "
                            + " SUM(B.DISB_AMT) DISB_AMT   "
                            + " FROM TM_LOAN_ALL A , GM_LOAN_TRANS B , MM_CUSTOMER C "
                            + " WHERE A.LOAN_ID = B.LOAN_ID           "
                            + " AND A.PARTY_CD  = C.CUST_CD           "
                            + " AND B.TRANS_DT BETWEEN to_date('{0}', 'dd-mm-yyyy') AND to_date('{1}', 'dd-mm-yyyy') "
                            + " AND B.TRANS_TYPE = 'B'                "
                            + " AND A.BRN_CD = {2}                    "
                            + " GROUP BY A.LOAN_ID, A.ACC_CD, B.TRANS_DT, A.PARTY_CD, C.CUST_NAME, A.BRN_CD ";



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
                                      prp.from_dt != null ? prp.from_dt.ToString("dd/MM/yyyy") : "TRANS_DT",
                                      prp.to_dt != null ? prp.to_dt.ToString("dd/MM/yyyy") : "TRANS_DT",
                                      string.IsNullOrWhiteSpace(prp.brn_cd) ? "BRN_CD" : string.Concat("'", prp.brn_cd , "'"));

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanDis = new tm_loan_all();

                                        loanDis.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                        loanDis.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                        loanDis.acc_cd = UtilityM.CheckNull<Int32>(reader["ACC_CD"]);
                                        loanDis.trans_dt = UtilityM.CheckNull<DateTime>(reader["TRANS_DT"]);
                                        loanDis.party_cd = UtilityM.CheckNull<decimal>(reader["PARTY_CD"]);
                                        loanDis.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                        loanDis.disb_amt = UtilityM.CheckNull<decimal>(reader["DISB_AMT"]);
                                        loanDisReg.Add(loanDis);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanDisReg = null;
                    }
                }
            }
            return loanDisReg;
        }




        internal List<gm_loan_trans> PopulateLoanStatement(p_report_param prp)
        {
            List<gm_loan_trans> loanStmtList = new List<gm_loan_trans>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _query = "SELECT GM_LOAN_TRANS.LOAN_ID, "
                + " GM_LOAN_TRANS.TRANS_DT,             "
                + " GM_LOAN_TRANS.TRANS_CD,             "
                + " GM_LOAN_TRANS.DISB_AMT,             "
                + " GM_LOAN_TRANS.CURR_PRN_RECOV,       "
                + " GM_LOAN_TRANS.OVD_PRN_RECOV,        "
                + " GM_LOAN_TRANS.CURR_INTT_RECOV,      "
                + " GM_LOAN_TRANS.OVD_INTT_RECOV,       "
                + " GM_LOAN_TRANS.LAST_INTT_CALC_DT,    "
                + " GM_LOAN_TRANS.CURR_INTT_CALCULATED, "
                + " GM_LOAN_TRANS.OVD_INTT_CALCULATED,  "
                + " GM_LOAN_TRANS.PRN_TRF,              "
                + " GM_LOAN_TRANS.INTT_TRF,             "
                + " GM_LOAN_TRANS.PRN_TRF_REVERT,       "
                + " GM_LOAN_TRANS.INTT_TRF_REVERT,      "
                + " GM_LOAN_TRANS.CURR_PRN,             "
                + " GM_LOAN_TRANS.OVD_PRN,              "
                + " GM_LOAN_TRANS.CURR_INTT,            "
                + " GM_LOAN_TRANS.OVD_INTT ,            "
                + " C.CUST_NAME                         "
                + " FROM GM_LOAN_TRANS , TM_LOAN_ALL A ,  MM_CUSTOMER C "
                + " WHERE GM_LOAN_TRANS.BRN_CD = {0}    "
                + " AND GM_LOAN_TRANS.LOAN_ID = {1}     "
                + " AND (GM_LOAN_TRANS.TRANS_TYPE) IN ('B', 'R', 'I') "
                + " AND GM_LOAN_TRANS.TRANS_DT BETWEEN to_date('{2}', 'dd-mm-yyyy') AND to_date('{3}', 'dd-mm-yyyy') "
                + " AND GM_LOAN_TRANS.LOAN_ID = A.LOAN_ID "
                + " AND A.PARTY_CD = C.CUST_CD ";


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
                                      string.IsNullOrWhiteSpace(prp.brn_cd) ? "BRN_CD" : string.Concat("'", prp.brn_cd, "'"),
                                      string.IsNullOrWhiteSpace(prp.loan_id) ? "LOAN_ID" : string.Concat("'", prp.loan_id , "'"),
                                      prp.from_dt != null ? prp.from_dt.ToString("dd/MM/yyyy") : "TRANS_DT",
                                      prp.to_dt != null ? prp.to_dt.ToString("dd/MM/yyyy") : "TRANS_DT"  );

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanStmt = new gm_loan_trans();

                                        loanStmt.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                        loanStmt.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                        loanStmt.trans_dt = UtilityM.CheckNull<DateTime>(reader["TRANS_DT"]);
                                        loanStmt.trans_cd = UtilityM.CheckNull<Int64>(reader["TRANS_CD"]);
                                        loanStmt.disb_amt = UtilityM.CheckNull<decimal>(reader["DISB_AMT"]);
                                        loanStmt.curr_prn_recov = UtilityM.CheckNull<decimal>(reader["CURR_PRN_RECOV"]);
                                        loanStmt.ovd_prn_recov = UtilityM.CheckNull<decimal>(reader["OVD_PRN_RECOV"]);
                                        loanStmt.curr_intt_recov = UtilityM.CheckNull<decimal>(reader["CURR_INTT_RECOV"]);
                                        loanStmt.ovd_intt_recov = UtilityM.CheckNull<decimal>(reader["OVD_INTT_RECOV"]);
                                        loanStmt.last_intt_calc_dt = UtilityM.CheckNull<DateTime>(reader["LAST_INTT_CALC_DT"]);
                                        loanStmt.curr_intt_calculated = UtilityM.CheckNull<decimal>(reader["CURR_INTT_CALCULATED"]);
                                        loanStmt.ovd_intt_calculated = UtilityM.CheckNull<decimal>(reader["OVD_INTT_CALCULATED"]);
                                        loanStmt.prn_trf = UtilityM.CheckNull<decimal>(reader["PRN_TRF"]);
                                        loanStmt.intt_trf = UtilityM.CheckNull<decimal>(reader["INTT_TRF"]);
                                        loanStmt.prn_trf_revert = UtilityM.CheckNull<decimal>(reader["PRN_TRF_REVERT"]);
                                        loanStmt.intt_trf_revert = UtilityM.CheckNull<decimal>(reader["INTT_TRF_REVERT"]);
                                        loanStmt.curr_prn = UtilityM.CheckNull<decimal>(reader["CURR_PRN"]);
                                        loanStmt.ovd_prn = UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                        loanStmt.curr_intt = UtilityM.CheckNull<decimal>(reader["CURR_INTT"]);
                                        loanStmt.ovd_intt = UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);

                                        loanStmtList.Add(loanStmt);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanStmtList = null;
                    }
                }
            }
            return loanStmtList;
        }



        internal List<gm_loan_trans> PopulateRecoveryRegister(p_report_param prp)
        {
            List<gm_loan_trans> loanRecoList = new List<gm_loan_trans>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _query = " SELECT A.BRN_CD,                       "
               + " A.LOAN_ID,                                        "
               + " A.ACC_CD,                                         "
               + " A.PARTY_CD,                                       "
               + " C.CUST_NAME,                                      "
               + " A.LAST_INTT_CALC_DT,                              "
               + " B.TRANS_DT,                                       "
               + " SUM(B.RECOV_AMT) RECOV_AMT,                       "
               + " SUM(B.CURR_PRN_RECOV) CURR_PRN_RECOV,             "
               + " SUM(B.OVD_PRN_RECOV) OVD_PRN_RECOV,               "
               + " SUM(B.CURR_INTT_RECOV) CURR_INTT_RECOV,           "
               + " SUM(B.OVD_INTT_RECOV) OVD_INTT_RECOV              "
               + " FROM TM_LOAN_ALL A, GM_LOAN_TRANS B, MM_CUSTOMER C "
               + " WHERE A.LOAN_ID = B.LOAN_ID                       "
               + " AND A.PARTY_CD  = C.CUST_CD                        "
               + " AND B.TRANS_DT BETWEEN to_date('{0}', 'dd-mm-yyyy') AND to_date('{1}', 'dd-mm-yyyy')"
               + " AND B.TRANS_TYPE = 'R'                            "
               + " AND A.BRN_CD = {2}                                "
               + " GROUP BY A.BRN_CD, A.LOAN_ID, A.ACC_CD, C.CUST_NAME,"
               + " A.PARTY_CD, A.LAST_INTT_CALC_DT, B.TRANS_DT       ";

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
                                      prp.from_dt != null ? prp.from_dt.ToString("dd/MM/yyyy") : "TRANS_DT",
                                      prp.to_dt != null ? prp.to_dt.ToString("dd/MM/yyyy") : "TRANS_DT",
                                      string.IsNullOrWhiteSpace(prp.brn_cd) ? "BRN_CD" : string.Concat("'", prp.brn_cd, "'"));

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanReco = new gm_loan_trans();


                                        loanReco.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                        loanReco.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                        loanReco.acc_cd = UtilityM.CheckNull<Int32>(reader["ACC_CD"]);
                                        loanReco.party_cd = UtilityM.CheckNull<decimal>(reader["PARTY_CD"]);
                                        loanReco.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                        loanReco.last_intt_calc_dt = UtilityM.CheckNull<DateTime>(reader["LAST_INTT_CALC_DT"]);
                                        loanReco.trans_dt = UtilityM.CheckNull<DateTime>(reader["TRANS_DT"]);
                                        loanReco.recov_amt = UtilityM.CheckNull<decimal>(reader["RECOV_AMT"]);
                                        loanReco.curr_prn_recov = UtilityM.CheckNull<decimal>(reader["CURR_PRN_RECOV"]);
                                        loanReco.ovd_prn_recov = UtilityM.CheckNull<decimal>(reader["OVD_PRN_RECOV"]);
                                        loanReco.curr_intt_recov = UtilityM.CheckNull<decimal>(reader["CURR_INTT_RECOV"]);
                                        loanReco.ovd_intt_recov = UtilityM.CheckNull<decimal>(reader["OVD_INTT_RECOV"]);

                                        loanRecoList.Add(loanReco);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanRecoList = null;
                    }
                }
            }
            return loanRecoList;
        }


        internal List<tt_loan_sub_cash_book> PopulateLoanSubCashBook(p_report_param prp)
        {
            List<tt_loan_sub_cash_book> loanSubCashBookList = new List<tt_loan_sub_cash_book>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "p_sub_csh_bk_loan";
            string _query = " SELECT TT_LOAN_SUB_CASH_BOOK.ACC_TYPE_CD,  "
               + " TT_LOAN_SUB_CASH_BOOK.ACC_NUM,  "
               + " TT_LOAN_SUB_CASH_BOOK.CASH_DR,  "
               + " TT_LOAN_SUB_CASH_BOOK.TRF_DR,   "
               + " TT_LOAN_SUB_CASH_BOOK.CASH_CR,  "
               + " TT_LOAN_SUB_CASH_BOOK.TRF_CR,   "
               + " TT_LOAN_SUB_CASH_BOOK.CUST_NAME "
               + " FROM TT_LOAN_SUB_CASH_BOOK      ";

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

                            var parm1 = new OracleParameter("as_brn_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm1.Value = prp.brn_cd;
                            command.Parameters.Add(parm1);

                            var parm2 = new OracleParameter("adt_as_on_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm2.Value = prp.adt_as_on_dt;
                            command.Parameters.Add(parm2);

                            command.ExecuteNonQuery();

                        }

                        _statement = string.Format(_query);

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanSubCashBook = new tt_loan_sub_cash_book();

                                        loanSubCashBook.acc_type_cd = Convert.ToInt32(UtilityM.CheckNull<Int32>(reader["ACC_TYPE_CD"]));
                                        loanSubCashBook.acc_num = UtilityM.CheckNull<string>(reader["ACC_NUM"]);
                                        loanSubCashBook.cash_dr = UtilityM.CheckNull<decimal>(reader["CASH_DR"]);
                                        loanSubCashBook.trf_dr = UtilityM.CheckNull<decimal>(reader["TRF_DR"]);
                                        loanSubCashBook.cash_cr = UtilityM.CheckNull<decimal>(reader["CASH_CR"]);
                                        loanSubCashBook.trf_cr = UtilityM.CheckNull<decimal>(reader["TRF_CR"]);
                                        loanSubCashBook.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);

                                        loanSubCashBookList.Add(loanSubCashBook);
                                    }
                                    transaction.Commit();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanSubCashBookList = null;
                    }
                }
            }
            return loanSubCashBookList;
        }


        internal List<tt_detailed_list_loan> GetDefaultList(p_report_param prp)
        {
            List<tt_detailed_list_loan> loanDfltList = new List<tt_detailed_list_loan>();

            string _alter = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY HH24:MI:SS'";
            string _statement;
            string _procedure = "p_detailed_list_loan";
            string _query = " SELECT TT_DETAILED_LIST_LOAN.ACC_CD,             "
                           + " TT_DETAILED_LIST_LOAN.PARTY_NAME,               "
                           + " TT_DETAILED_LIST_LOAN.CURR_INTT_RATE,           "
                           + " TT_DETAILED_LIST_LOAN.OVD_INTT_RATE,            "
                           + " TT_DETAILED_LIST_LOAN.CURR_PRN,                 "
                           + " TT_DETAILED_LIST_LOAN.OVD_PRN,                  "
                           + " TT_DETAILED_LIST_LOAN.CURR_INTT,                "
                           + " TT_DETAILED_LIST_LOAN.OVD_INTT,                 "
                           + " TT_DETAILED_LIST_LOAN.ACC_NAME,                 "
                           + " TT_DETAILED_LIST_LOAN.ACC_NUM,                  "
                           + " TT_DETAILED_LIST_LOAN.BLOCK_NAME,               "
                           + " TT_DETAILED_LIST_LOAN.COMPUTED_TILL_DT          "
                           + " FROM TT_DETAILED_LIST_LOAN                      "
                           + " WHERE   ( TT_DETAILED_LIST_LOAN.ACC_CD = '{0}' )  "
                           + " AND     ( TT_DETAILED_LIST_LOAN.OVD_PRN > 0  )  ";

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

                            var parm1 = new OracleParameter("as_brn_cd", OracleDbType.Varchar2, ParameterDirection.Input);
                            parm1.Value = prp.brn_cd;
                            command.Parameters.Add(parm1);

                            var parm2 = new OracleParameter("ad_acc_cd", OracleDbType.Int32, ParameterDirection.Input);
                            parm2.Value = prp.acc_cd;
                            command.Parameters.Add(parm2);

                            var parm3 = new OracleParameter("adt_dt", OracleDbType.Date, ParameterDirection.Input);
                            parm3.Value = prp.adt_dt;
                            command.Parameters.Add(parm3);

                            command.ExecuteNonQuery();

                        }

                        _statement = string.Format(_query,
                                                   prp.acc_cd != 0 ? Convert.ToString(prp.acc_cd) : "ACC_CD");

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var loanDtl = new tt_detailed_list_loan();

                                        loanDtl.acc_cd = Convert.ToInt32(UtilityM.CheckNull<Int32>(reader["ACC_CD"]));
                                        loanDtl.party_name = UtilityM.CheckNull<string>(reader["PARTY_NAME"]);
                                        loanDtl.curr_intt_rate = UtilityM.CheckNull<float>(reader["CURR_INTT_RATE"]);
                                        loanDtl.ovd_intt_rate = UtilityM.CheckNull<float>(reader["OVD_INTT_RATE"]);
                                        loanDtl.curr_prn = UtilityM.CheckNull<decimal>(reader["CURR_PRN"]);
                                        loanDtl.ovd_prn = UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                        loanDtl.curr_intt = UtilityM.CheckNull<decimal>(reader["CURR_INTT"]);
                                        loanDtl.ovd_intt = UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);
                                        loanDtl.acc_name = UtilityM.CheckNull<string>(reader["ACC_NAME"]);
                                        loanDtl.acc_num = UtilityM.CheckNull<string>(reader["ACC_NUM"]);
                                        loanDtl.block_name = UtilityM.CheckNull<string>(reader["BLOCK_NAME"]);
                                        loanDtl.computed_till_dt = UtilityM.CheckNull<DateTime>(reader["COMPUTED_TILL_DT"]);
                                        loanDtl.list_dt = UtilityM.CheckNull<DateTime>(reader["COMPUTED_TILL_DT"]);
                                        loanDfltList.Add(loanDtl);
                                    }
                                    transaction.Commit();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        loanDfltList = null;
                    }
                }
            }
            return loanDfltList;
        }


    }
}
