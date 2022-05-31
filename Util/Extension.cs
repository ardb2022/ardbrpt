//using Oracle.DataAccess.Client;
using SBWSFinanceApi.LL;
using SBWSFinanceApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Oracle.ManagedDataAccess.Client;

namespace RDLCReportServer.Util
{
    public static class Extension
    {
        public static DataSet ToDataSet<T>(this IList<T> list)
        {
            Type elementType = typeof(T);
            DataSet ds = new DataSet();
            DataTable t = new DataTable();
            ds.Tables.Add(t);

            foreach (var propInfo in elementType.GetProperties())
            //add a column to table for each public property on Tforeach (var propInfo in elementType.GetProperties())
            {
                Type ColType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;

                t.Columns.Add(propInfo.Name, ColType);
            }

            foreach (T item in list)
            //go through each property on T and add each value to the tableforeach (T item in list)
            {
                DataRow row = t.NewRow();

                foreach (var propInfo in elementType.GetProperties())
                {
                    row[propInfo.Name] = propInfo.GetValue(item, null) ?? DBNull.Value;
                }

                t.Rows.Add(row);
            }

            return ds;
        }
       
    }
    internal static class UtilityM
    {
        internal static T CheckNull<T>(object obj)
        {
            return (obj == DBNull.Value ? default(T) : (T)obj);
        }
    }

    public static class OrclDbConnection
    {
       
        public static DbCommand Command(DbConnection connection, string cmdText)
        {
            return new OracleCommand(cmdText, (OracleConnection)connection);
        }

        public static DbConnection NewConnectionAdmin
        {
            get
            {
                #region New Logic
                //var conns = _config.GetSection("DbConnections").Get<BankConfig>();
                BankConfigMst BC = new BankConfigMstLL().ReadAllConfiguration();
                OracleConnectionStringBuilder sb = new OracleConnectionStringBuilder();
                //sb.DataSource = "10.65.65.246:1521/orcl"; // EZ Connect -- no TNS Names!
                //sb.UserID = "admin_master_dev";
                //sb.Password = "signature";

                sb.DataSource = BC.DbConnections.db_server_ip;
                sb.UserID = BC.DbConnections.user1;
                sb.Password = BC.DbConnections.pass1;
                #endregion

                DbConnection connection = null;
                try
                {

                    connection = new OracleConnection(sb.ToString());
                    connection.Open();
                }
                catch (Exception ex)
                {
                    connection.Close();
                    throw ex;
                }

                return connection;
            }
        }

        public static DbConnection NewConnection
        {
            get
            {
                //BankConfigMst BC = new BankConfigMstLL().ReadAllConfiguration();
                BankConfig BC = getBankConfigFromDB();
                OracleConnectionStringBuilder sb = new OracleConnectionStringBuilder();
                // Use below 3 for DEV
                //sb.DataSource = "10.65.65.246:1521/orcl"; // EZ Connect -- no TNS Names!
                //sb.UserID = "cbs_demo";
                //sb.Password = "signature";

                // Use below 3 for PRD deploymen/t
                sb.DataSource = BC.db_server_ip;
                sb.UserID = BC.user1;
                sb.Password = BC.pass1;

                //sb.DataSource="202.65.156.246:1521/orcl";
                DbConnection connection = null;
                try
                {
                    Console.WriteLine(sb.ToString());
                    connection = new OracleConnection(sb.ToString());
                    connection.Open();
                }
                catch (Exception ex)
                {
                    connection.Close();
                    throw ex;
                }

                return connection;
            }
        }
        internal static BankConfig getBankConfigFromDB()
        {
            BankConfig bankConfigToRtrn = null;
            string bankName = System.AppDomain.CurrentDomain.BaseDirectory;
            if (null != bankName)
            {
                //// PRD
                //bankName = bankName.Substring(0,bankName.Length - 5);
                //var folderName = bankName.Split('\\').LastOrDefault();
                //folderName = "DEVUX";
                Console.WriteLine(bankName);
                var splittedStrings = bankName.Split('\\');
                var folderName = splittedStrings[splittedStrings.Length - 2];
                Console.WriteLine(folderName);
                if (folderName == "RPT")
                {
                    if (splittedStrings.Length > 1)
                    {
                        folderName = splittedStrings[splittedStrings.Length - 3];
                        Console.WriteLine(folderName);
                    }
                }
                DbConnection connection = NewConnectionAdmin;
                try
                {

                    string _query = " SELECT BANK_NAME "
                                                    + ",DB_SERVER_IP ,USER1 ,PASS1 ,USER2 "
                                                    + ",PASS2 ,UPDATED_DT "
                                                    + " FROM BANK_CONFIG WHERE ACTIVE_FLAG = 'Y' "
                                                    + " AND BANK_NAME = '{0}' ";
                    _query = string.Format(_query, folderName);
                    using (var command = OrclDbConnection.Command(connection, _query))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                {
                                    while (reader.Read())
                                    {
                                        bankConfigToRtrn = new BankConfig();
                                        bankConfigToRtrn.bank_name = UtilityM.CheckNull<string>(reader["BANK_NAME"]);
                                        bankConfigToRtrn.db_server_ip = UtilityM.CheckNull<string>(reader["DB_SERVER_IP"]);
                                        bankConfigToRtrn.user1 = UtilityM.CheckNull<string>(reader["USER1"]);
                                        bankConfigToRtrn.pass1 = UtilityM.CheckNull<string>(reader["PASS1"]);
                                        bankConfigToRtrn.user2 = UtilityM.CheckNull<string>(reader["USER2"]);
                                        bankConfigToRtrn.pass2 = UtilityM.CheckNull<string>(reader["PASS2"]);
                                        bankConfigToRtrn.updated_dt = UtilityM.CheckNull<DateTime>(reader["UPDATED_DT"]);
                                    }
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    if (null != connection && connection.State != System.Data.ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                    throw ex;
                }
                finally
                {
                    if (null != connection && connection.State != System.Data.ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                }

            }

            return bankConfigToRtrn;
        }


    }
}

