using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using RDLCReportServer.Model;
using RDLCReportServer.Util;
using SBWSFinanceApi.Models;


namespace SBWSFinanceApi.DL
{
    internal class BankConfigMstDL
    {
        string _statement ="";
        private string path
        {
            get
            {
               // return Directory.GetCurrentDirectory() + @"\Constant\BranchConfig.json";
                return HttpContext.Current.Server.MapPath(@"~/Constant/appsettings.json");
            }
        }
        // private string path = @"D:\POC\DreamBig\SSS\Banking\SBWS\SBWSFinanceApi\Constant\BranchConfig.json";
        internal BankConfigMst ReadAllConfiguration()
        {
            // Console.WriteLine(path);
            BankConfigMst bankConfig = new BankConfigMst();


            // var dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            // var path = Path.Combine(dir, "BranchConfig.json");
           // var path = HttpContext.Current.Server.MapPath(@"~/App_Data/BranchConfig.json");
           // string fileContent = File.ReadAllText(path);
           // bankConfig = JsonConvert.DeserializeObject<BankConfigMst>(fileContent);
            if (File.Exists(path))
            {
            string fileContent = File.ReadAllText(path);
            bankConfig = JsonConvert.DeserializeObject<BankConfigMst>(fileContent);

            }

            return bankConfig;
        }

        internal string GetBranchMaster(string brn_cd)
        {
            string brn_name="";
            string brn_addr = "";
            string _query = " SELECT BRN_CD, BRN_NAME,BRN_ADDR,BRN_IFSC_CODE,IP_ADDRESS"
                         + " FROM M_BRANCH WHERE BRN_CD={0}";
            using (var connection = OrclDbConnection.NewConnection)
            {
                _statement = string.Format(_query,
                                           !string.IsNullOrWhiteSpace(brn_cd) ? string.Concat("'", brn_cd, "'") : "BRN_CD"
                                           );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                brn_name = UtilityM.CheckNull<string>(reader["BRN_NAME"]);
                                brn_addr = UtilityM.CheckNull<string>(reader["BRN_ADDR"]);
                            }
                        }
                    }
                }

            }
            return brn_name + " - "+ brn_addr;
        }

        internal List<mm_category> GetCategoryMaster()
        {
            List<mm_category> mamRets = new List<mm_category>();
            string _query = " SELECT CATG_CD, CATG_DESC"
                         + " FROM MM_CATEGORY";
            using (var connection = OrclDbConnection.NewConnection)
            {
                _statement = string.Format(_query);
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var mam = new mm_category();
                                mam.catg_cd = UtilityM.CheckNull<int>(reader["CATG_CD"]);
                                mam.catg_desc = UtilityM.CheckNull<string>(reader["CATG_DESC"]);
                                mamRets.Add(mam);
                            }
                        }
                    }
                }

            }
            return mamRets;
        }

        internal List<mm_acc_type> GetAccountTypeMaster()
        {
            List<mm_acc_type> mamRets = new List<mm_acc_type>();
            string _query = " SELECT  ACC_TYPE_CD,ACC_TYPE_DESC,TRANS_WAY,DEP_LOAN_FLAG,INTT_TRF_TYPE,REP_SCH_FLAG FROM MM_ACC_TYPE";

            using (var connection = OrclDbConnection.NewConnection)
            {
                _statement = string.Format(_query);
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var mam = new mm_acc_type();
                                mam.acc_type_cd = UtilityM.CheckNull<int>(reader["ACC_TYPE_CD"]);
                                mam.acc_type_desc = UtilityM.CheckNull<string>(reader["ACC_TYPE_DESC"]);
                                mam.trans_way = UtilityM.CheckNull<string>(reader["TRANS_WAY"]);
                                mam.dep_loan_flag = UtilityM.CheckNull<string>(reader["DEP_LOAN_FLAG"]);
                                mam.intt_trf_type = UtilityM.CheckNull<string>(reader["INTT_TRF_TYPE"]);
                                mam.rep_sch_flag = UtilityM.CheckNull<string>(reader["REP_SCH_FLAG"]);
                                mamRets.Add(mam);
                            }
                        }
                    }
                }

            }
            return mamRets;
        }

        internal List<m_acc_master> GetAccountMaster()
        {
            List<m_acc_master> mamRets = new List<m_acc_master>();
            string _query = " SELECT SCHEDULE_CD, SUB_SCHEDULE_CD, ACC_CD, ACC_NAME, ACC_TYPE, IMPL_FLAG, ONLINE_FLAG, MIS_ACC_CD, TRADING_FLAG, STOCK_CD, N_TRIAL_CD"
                         + " FROM M_ACC_MASTER";
            using (var connection = OrclDbConnection.NewConnection)
            {
                _statement = string.Format(_query);
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var mam = new m_acc_master();
                                mam.schedule_cd = UtilityM.CheckNull<int>(reader["SCHEDULE_CD"]);
                                mam.sub_schedule_cd = UtilityM.CheckNull<int>(reader["SUB_SCHEDULE_CD"]);
                                mam.acc_cd = UtilityM.CheckNull<int>(reader["ACC_CD"]);
                                mam.acc_name = UtilityM.CheckNull<string>(reader["ACC_NAME"]);
                                mam.acc_type = UtilityM.CheckNull<string>(reader["ACC_TYPE"]);
                                mam.impl_flag = UtilityM.CheckNull<string>(reader["IMPL_FLAG"]);
                                mam.online_flag = UtilityM.CheckNull<string>(reader["ONLINE_FLAG"]);
                                mam.mis_acc_cd = UtilityM.CheckNull<int>(reader["MIS_ACC_CD"]);
                                mam.trading_flag = UtilityM.CheckNull<string>(reader["TRADING_FLAG"]);
                                mam.stock_cd = UtilityM.CheckNull<int>(reader["STOCK_CD"]);
                                mam.n_trial_cd = UtilityM.CheckNull<int>(reader["N_TRIAL_CD"]);
                                mamRets.Add(mam);
                            }
                        }
                    }
                }

            }
            return mamRets;
        }

        internal List<mm_constitution> GetConstitution()
        {
            List<mm_constitution> mamRets = new List<mm_constitution>();
            string _query = "SELECT  ACC_TYPE_CD,CONSTITUTION_CD,CONSTITUTION_DESC,ACC_CD,INTT_ACC_CD,INTT_PROV_ACC_CD,ALLOW_TRANS FROM MM_CONSTITUTION";

            using (var connection = OrclDbConnection.NewConnection)
            {
                _statement = string.Format(_query);
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var mam = new mm_constitution();
                                mam.acc_type_cd = UtilityM.CheckNull<int>(reader["ACC_TYPE_CD"]);
                                mam.constitution_cd = UtilityM.CheckNull<int>(reader["CONSTITUTION_CD"]);
                                mam.constitution_desc = UtilityM.CheckNull<string>(reader["CONSTITUTION_DESC"]);
                                mam.acc_cd = UtilityM.CheckNull<int>(reader["ACC_CD"]);
                                mam.intt_acc_cd = UtilityM.CheckNull<int>(reader["INTT_ACC_CD"]);
                                mam.intt_prov_acc_cd = UtilityM.CheckNull<int>(reader["INTT_PROV_ACC_CD"]);
                                mam.allow_trans = UtilityM.CheckNull<string>(reader["ALLOW_TRANS"]);

                                mamRets.Add(mam);
                            }
                        }
                    }
                }

            }
            return mamRets;
        }


    }
}