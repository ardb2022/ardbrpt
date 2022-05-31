using RDLCReportServer.Util;
using SBWSFinanceApi.Models;
using System;
using System.Collections.Generic;

namespace SBWSFinanceApi.DL
{
    public class VoucherDL
    {
        string _statement;
        string narration_ret;
            
        internal List<t_voucher_dtls> GetTVoucherDtls(t_voucher_dtls tvd)
        {
            List<t_voucher_dtls> tvdRets=new List<t_voucher_dtls>();
            string _query="SELECT t_voucher_dtls.brn_cd,t_voucher_dtls.voucher_dt,t_voucher_dtls.acc_cd,"  
                          +"t_voucher_dtls.amount,t_voucher_dtls.debit_credit_flag,t_voucher_dtls.transaction_type,"
                          +"t_voucher_dtls.narration,m_acc_master.acc_name,t_voucher_dtls.approval_status,"
                          +"t_voucher_dtls.voucher_id,t_voucher_dtls.approved_by,t_voucher_dtls.approved_dt,"
                          +"t_voucher_dtls.amount dr_amount,0.00 cr_amount   FROM t_voucher_dtls,  m_acc_master WHERE ( t_voucher_dtls.acc_cd = m_acc_master.acc_cd ) AND"
                          +"( t_voucher_dtls.brn_cd = {0} ) AND ( t_voucher_dtls.voucher_dt = to_date('{1}','dd-mm-yyyy' )  ) AND "
                          +"( t_voucher_dtls.voucher_id = {2} )    AND ( t_voucher_dtls.debit_credit_flag = 'D' )  "
                          +" UNION  "
                          +"SELECT t_voucher_dtls.brn_cd,t_voucher_dtls.voucher_dt,t_voucher_dtls.acc_cd,"  
                          +"t_voucher_dtls.amount,t_voucher_dtls.debit_credit_flag,t_voucher_dtls.transaction_type,"
                          +"t_voucher_dtls.narration,m_acc_master.acc_name,t_voucher_dtls.approval_status,"
                          +"t_voucher_dtls.voucher_id,t_voucher_dtls.approved_by,t_voucher_dtls.approved_dt,"
                          +"0.00 dr_amount,t_voucher_dtls.amount cr_amount  FROM t_voucher_dtls,  m_acc_master WHERE ( t_voucher_dtls.acc_cd = m_acc_master.acc_cd ) AND"
                          +"( t_voucher_dtls.brn_cd = {0} ) AND ( t_voucher_dtls.voucher_dt = to_date('{1}','dd-mm-yyyy' )  ) AND "
                          +"( t_voucher_dtls.voucher_id = {2} )    AND ( t_voucher_dtls.debit_credit_flag = 'C' )  ";
             string _query1="SELECT NARRATION FROM T_VOUCHER_NARRATION WHERE  "
                          +" T_VOUCHER_NARRATION.brn_cd = {0} AND  T_VOUCHER_NARRATION.voucher_dt = to_date('{1}','dd-mm-yyyy' )  AND "
                          +" T_VOUCHER_NARRATION.voucher_id = {2} ";
            using (var connection = OrclDbConnection.NewConnection)
            {
              
                _statement = string.Format(_query,
                                            string.IsNullOrWhiteSpace( tvd.brn_cd) ? "brn_cd" : string.Concat("'",  tvd.brn_cd , "'"),
                                            tvd.voucher_dt!= null ? Convert.ToString(tvd.voucher_dt).Substring(0, 10): "voucher_dt",
                                            tvd.voucher_id !=0 ? Convert.ToString(tvd.voucher_id) : "voucher_id"
                                            );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)     
                        {
                            while (reader.Read())
                            {                                
                               var tvdr = new t_voucher_dtls();
                                tvdr.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                tvdr.voucher_dt = UtilityM.CheckNull<DateTime>(reader["VOUCHER_DT"]);
                                tvdr.acc_cd = UtilityM.CheckNull<int>(reader["ACC_CD"]);
                                tvdr.amount = UtilityM.CheckNull<decimal>(reader["AMOUNT"]);
                                tvdr.debit_credit_flag = UtilityM.CheckNull<string>(reader["DEBIT_CREDIT_FLAG"]);
                                tvdr.transaction_type = UtilityM.CheckNull<string>(reader["TRANSACTION_TYPE"]);
                                tvdr.narration = UtilityM.CheckNull<string>(reader["NARRATION"]);
                                tvdr.acc_name = UtilityM.CheckNull<string>(reader["ACC_NAME"]);
                                tvdr.approval_status = UtilityM.CheckNull<string>(reader["APPROVAL_STATUS"]);
                                tvdr.voucher_id = UtilityM.CheckNull<int>(reader["VOUCHER_ID"]);
                                tvdr.approved_by = UtilityM.CheckNull<string>(reader["APPROVED_BY"]);
                                tvdr.approved_dt = UtilityM.CheckNull<DateTime>(reader["APPROVED_DT"]);
                                tvdr.dr_amount = UtilityM.CheckNull<decimal>(reader["DR_AMOUNT"]);
                                tvdr.cr_amount = UtilityM.CheckNull<decimal>(reader["CR_AMOUNT"]);
                                tvdRets.Add(tvdr);
                            }
                        }
                    }
                }
                
                _statement = string.Format(_query1,
                                            string.IsNullOrWhiteSpace( tvd.brn_cd) ? "brn_cd" : string.Concat("'",  tvd.brn_cd , "'"),
                                            tvd.voucher_dt!= null ? Convert.ToString(tvd.voucher_dt).Substring(0, 10): "voucher_dt",
                                            tvd.voucher_id !=0 ? Convert.ToString(tvd.voucher_id) : "voucher_id"
                                            );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)     
                        {
                            while (reader.Read())
                            {                                
                                narration_ret = UtilityM.CheckNull<string>(reader["NARRATION"]);
                            }
                        }
                    }
                }

                if(tvdRets.Count>0)
                tvdRets[0].narrationdtl=narration_ret;
            
            }

            return tvdRets;
        }  

        internal List<t_voucher_dtls> GetTVoucherNarration(t_voucher_dtls tvd)
        {
             List<t_voucher_dtls> tvdRets=new List<t_voucher_dtls>();
            string _query="SELECT NARRATION,VOUCHER_ID,VOUCHER_DT,VOUCHER_TYPE,APPROVAL_STATUS FROM T_VOUCHER_NARRATION WHERE  "
                          +"  T_VOUCHER_NARRATION.voucher_dt = to_date('{0}','dd-mm-yyyy' )  AND "
                          +"  T_VOUCHER_NARRATION.brn_cd = {1} ";
                          
            using (var connection = OrclDbConnection.NewConnection)
            {              
                _statement = string.Format(_query,
                                            tvd.voucher_dt!= null ? Convert.ToString(tvd.voucher_dt).Substring(0, 10): "voucher_dt",
                                            string.IsNullOrWhiteSpace( tvd.brn_cd) ? "brn_cd" : string.Concat("'",  tvd.brn_cd , "'")
                                            );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)     
                        {
                            while (reader.Read())
                            {                     
                               var tvdr = new t_voucher_dtls();
                               tvdr.narrationdtl = UtilityM.CheckNull<string>(reader["NARRATION"]);
                               tvdr.voucher_id = UtilityM.CheckNull<int>(reader["VOUCHER_ID"]);
                                tvdr.voucher_dt = UtilityM.CheckNull<DateTime>(reader["VOUCHER_DT"]);
                                tvdr.transaction_type = UtilityM.CheckNull<string>(reader["VOUCHER_TYPE"]);
                                tvdr.approval_status = UtilityM.CheckNull<string>(reader["APPROVAL_STATUS"]);
                                tvdRets.Add(tvdr);
                            }
                        }
                    }
                }            
            }

            return tvdRets;
        }      

    internal int InsertTVoucherDtls(List<t_voucher_dtls> tvd)
    {
            int _ret=0;
            List<t_voucher_dtls> tvdRets=new List<t_voucher_dtls>();
            string _query="INSERT INTO T_VOUCHER_DTLS (BRN_CD, VOUCHER_DT, VOUCHER_ID, ACC_CD, AMOUNT, DEBIT_CREDIT_FLAG, TRANSACTION_TYPE, NARRATION, APPROVAL_STATUS)"
                            +" VALUES ({0},  to_date('{1}','dd-mm-yyyy' ), {2}, {3}, {4}, {5}, {6},  {7}, {8})";
            string _query1="INSERT INTO T_VOUCHER_NARRATION (BRN_CD, VOUCHER_DT, VOUCHER_ID, NARRATION, VOUCHER_TYPE, APPROVAL_STATUS)"
                            +"VALUES ({0},  to_date('{1}','dd-mm-yyyy' ), {2}, {3}, {4}, {5})";
            int VoucherIdMax=GetTVoucherDtlsMaxId(tvd[0]);
            using (var connection = OrclDbConnection.NewConnection)
            {
                 
                using (var transaction = connection.BeginTransaction())
                {               
                    try
                    {
                    for (int i=0;i<tvd.Count;i++)
                    {
                             _statement = string.Format(_query,
                                          string.Concat("'", tvd[i].brn_cd, "'"), 
                                          string.Concat(tvd[i].voucher_dt.ToString("dd/MM/yyyy")),
                                          string.Concat(VoucherIdMax),
                                          string.Concat("'", tvd[i].acc_cd, "'"),
                                          string.Concat( tvd[i].amount),
                                          string.Concat("'", tvd[i].debit_credit_flag, "'"),
                                          string.Concat("'", tvd[i].transaction_type, "'"),
                                          string.Concat("'", tvd[i].narration, "'"),
                                          string.Concat("'", tvd[i].approval_status, "'")
                                          );

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {                   
                            command.ExecuteNonQuery();
                            _ret=0;
                        }
                    }
                         _statement = string.Format(_query1,
                                          string.Concat("'", tvd[0].brn_cd, "'"), 
                                          string.Concat(tvd[0].voucher_dt.ToString("dd/MM/yyyy")),
                                           string.Concat(VoucherIdMax),
                                          string.Concat("'", tvd[0].narrationdtl, "'"),
                                          string.Concat("'", tvd[0].transaction_type, "'"),
                                          string.Concat("'", tvd[0].approval_status, "'")
                                          );

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {                   
                            command.ExecuteNonQuery();
                            transaction.Commit();
                            _ret=VoucherIdMax;
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _ret=-1;
                    }
                }
            }
        return _ret;
    }

    internal int GetTVoucherDtlsMaxId(t_voucher_dtls tvd)
        {
            int maxVoucherId=0;
            string _query="Select Nvl(max(voucher_id) + 1, 1) max_voucher_id"
                            +" From   t_voucher_narration"
                            +" Where  voucher_dt =  to_date('{0}','dd-mm-yyyy' ) "
	                        +" And    brn_cd = {1}";
            using (var connection = OrclDbConnection.NewConnection)
            {              
                _statement = string.Format(_query,
                                            tvd.voucher_dt!= null ? Convert.ToString(tvd.voucher_dt).Substring(0, 10): "cr_dt",
                                            string.IsNullOrWhiteSpace( tvd.brn_cd) ? "brn_cd" : string.Concat("'",  tvd.brn_cd , "'")
                                            );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)     
                        {
                            while (reader.Read())
                            {                     
                               maxVoucherId = Convert.ToInt32(UtilityM.CheckNull<decimal>(reader["MAX_VOUCHER_ID"]));
                            }
                        }
                    }
                }            
            }

            return maxVoucherId;
        }

        internal int UpdateTVoucherDtls(List<t_voucher_dtls> tvd)
        {
            int _ret=0;
            string _query="Update t_voucher_dtls"
                            +" Set approved_by = {0} , approval_status ={1}, approved_dt=to_date('{2}','dd-mm-yyyy' )"
                            +" Where  brn_cd = {3}  AND voucher_dt = to_date('{4}','dd-mm-yyyy' ) AND "
                            +" voucher_id = {5} AND acc_cd= {6}";
            string _query1="Update t_voucher_narration"
                            +" Set narration = {0} , approval_status ={1}"
                            +" Where  brn_cd = {2}  AND voucher_dt = to_date('{3}','dd-mm-yyyy' ) AND "
                            +" voucher_id = {4} ";
            using (var connection = OrclDbConnection.NewConnection)
            {              
                 using (var transaction = connection.BeginTransaction())
                {               
                    try
                    {
                    for (int i=0;i<tvd.Count;i++)
                    {
                             _statement = string.Format(_query,
                                            string.Concat("'", tvd[i].approved_by, "'"),
                                            string.Concat("'", tvd[i].approval_status, "'"),
                                            string.Concat(tvd[i].approved_dt.ToString("dd/MM/yyyy")),
                                            string.IsNullOrWhiteSpace( tvd[i].brn_cd) ? "brn_cd" : string.Concat("'",  tvd[i].brn_cd , "'"),
                                            tvd[i].voucher_dt!= null ? tvd[i].voucher_dt.ToString("dd/MM/yyyy"): "voucher_dt",
                                            tvd[i].voucher_id !=0 ? Convert.ToString(tvd[i].voucher_id) : "voucher_id",
                                            tvd[i].acc_cd !=0 ? Convert.ToString(tvd[i].acc_cd) : "acc_cd"
                                          );
                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {                   
                            command.ExecuteNonQuery();
                            _ret=0;
                        }
                    }
                         _statement = string.Format(_query1,
                                            string.Concat("'", tvd[0].narrationdtl, "'"),
                                            string.Concat("'", tvd[0].approval_status, "'"),
                                            string.IsNullOrWhiteSpace( tvd[0].brn_cd) ? "brn_cd" : string.Concat("'",  tvd[0].brn_cd , "'"),
                                            tvd[0].voucher_dt!= null ? tvd[0].voucher_dt.ToString("dd/MM/yyyy"): "voucher_dt",
                                            tvd[0].voucher_id !=0 ? Convert.ToString(tvd[0].voucher_id) : "voucher_id"
                                          );

                        using (var command = OrclDbConnection.Command(connection, _statement))
                        {                   
                            command.ExecuteNonQuery();
                            transaction.Commit();
                            _ret=0;
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _ret=-1;
                    }
                }           
            }

            return _ret;
        }
}
}