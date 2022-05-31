using RDLCReportServer.Model;
using RDLCReportServer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RDLCReportServer.DL.UCIC
{
    public class CustomerDL
    {
        string _statement;
        internal List<mm_customer> GetCustomerDtls(mm_customer pmc)
        {
            List<mm_customer> custRets = new List<mm_customer>();
            string _query = "SELECT  MM_CUSTOMER.BRN_CD,"
         + " MM_CUSTOMER.CUST_CD,"
         + " MM_CUSTOMER.CUST_TYPE,"
         + " MM_CUSTOMER.TITLE,"
         + " MM_CUSTOMER.FIRST_NAME,"
         + " MM_CUSTOMER.MIDDLE_NAME,"
         + " MM_CUSTOMER.LAST_NAME,"
         + " MM_CUSTOMER.CUST_NAME,"
         + " MM_CUSTOMER.GUARDIAN_NAME,"
         + " MM_CUSTOMER.CUST_DT,"
         + " MM_CUSTOMER.OLD_CUST_CD,"
         + " MM_CUSTOMER.DT_OF_BIRTH,"
         + " MM_CUSTOMER.AGE,"
         + " MM_CUSTOMER.SEX,"
         + " MM_CUSTOMER.MARITAL_STATUS,"
         + " MM_CUSTOMER.CATG_CD,"
         + " MM_CUSTOMER.COMMUNITY,"
         + " MM_CUSTOMER.CASTE,"
         + " MM_CUSTOMER.PERMANENT_ADDRESS,"
         + " MM_CUSTOMER.WARD_NO,"
         + " MM_CUSTOMER.STATE,"
         + " MM_CUSTOMER.DIST,"
          + " MM_CUSTOMER.PIN,"
         + " MM_CUSTOMER.VILL_CD,"
         + " MM_CUSTOMER.BLOCK_CD,"
         + " MM_CUSTOMER.SERVICE_AREA_CD,"
         + " MM_CUSTOMER.OCCUPATION,"
         + " MM_CUSTOMER.PHONE,"
         + " MM_CUSTOMER.PRESENT_ADDRESS,"
         + " MM_CUSTOMER.FARMER_TYPE,"
         + " MM_CUSTOMER.EMAIL,"
         + " MM_CUSTOMER.MONTHLY_INCOME,"
         + " MM_CUSTOMER.DATE_OF_DEATH,"
         + " MM_CUSTOMER.SMS_FLAG,"
         + " MM_CUSTOMER.STATUS,"
         + " MM_CUSTOMER.PAN,"
         + " MM_CUSTOMER.NOMINEE,"
         + " MM_CUSTOMER.NOM_RELATION,"
         + " MM_CUSTOMER.KYC_PHOTO_TYPE,"
         + " MM_CUSTOMER.KYC_PHOTO_NO,"
         + " MM_CUSTOMER.KYC_ADDRESS_TYPE,"
         + " MM_CUSTOMER.KYC_ADDRESS_NO,"
         + " MM_CUSTOMER.ORG_STATUS,"
         + " MM_CUSTOMER.ORG_REG_NO,"
         + " MM_CUSTOMER.CREATED_BY,"
         + " MM_CUSTOMER.CREATED_DT,"
         + " MM_CUSTOMER.MODIFIED_BY,"
         + " MM_CUSTOMER.MODIFIED_DT"
    + " FROM  MM_CUSTOMER"
   + " WHERE MM_CUSTOMER.CUST_TYPE='M'"
   + " AND MM_CUSTOMER.BRN_CD={0}";
            using (var connection = OrclDbConnection.NewConnection)
            {

                _statement = string.Format(_query,
                                            !string.IsNullOrWhiteSpace(pmc.brn_cd) ? string.Concat("'", pmc.brn_cd, "'") : "MM_CUSTOMER.BRN_CD"
                                            );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var mc = new mm_customer();
                                mc.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                mc.cust_cd = UtilityM.CheckNull<decimal>(reader["CUST_CD"]);
                                mc.cust_type = UtilityM.CheckNull<string>(reader["CUST_TYPE"]);
                                mc.title = UtilityM.CheckNull<string>(reader["TITLE"]);
                                mc.first_name = UtilityM.CheckNull<string>(reader["FIRST_NAME"]);
                                mc.middle_name = UtilityM.CheckNull<string>(reader["MIDDLE_NAME"]);
                                mc.last_name = UtilityM.CheckNull<string>(reader["LAST_NAME"]);
                                mc.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                mc.guardian_name = UtilityM.CheckNull<string>(reader["GUARDIAN_NAME"]);
                                mc.cust_dt = UtilityM.CheckNull<DateTime>(reader["CUST_DT"]);
                                mc.old_cust_cd = UtilityM.CheckNull<string>(reader["OLD_CUST_CD"]);
                                mc.dt_of_birth = UtilityM.CheckNull<DateTime>(reader["DT_OF_BIRTH"]);
                                mc.age = UtilityM.CheckNull<decimal>(reader["AGE"]);
                                mc.sex = UtilityM.CheckNull<string>(reader["SEX"]);
                                mc.marital_status = UtilityM.CheckNull<string>(reader["MARITAL_STATUS"]);
                                mc.catg_cd = UtilityM.CheckNull<int>(reader["CATG_CD"]);
                                mc.community = UtilityM.CheckNull<decimal>(reader["COMMUNITY"]);
                                mc.caste = UtilityM.CheckNull<decimal>(reader["CASTE"]);
                                mc.permanent_address = UtilityM.CheckNull<string>(reader["PERMANENT_ADDRESS"]);
                                mc.ward_no = UtilityM.CheckNull<decimal>(reader["WARD_NO"]);
                                mc.state = UtilityM.CheckNull<string>(reader["STATE"]);
                                mc.dist = UtilityM.CheckNull<string>(reader["DIST"]);
                                mc.pin = UtilityM.CheckNull<int>(reader["PIN"]);
                                mc.vill_cd = UtilityM.CheckNull<string>(reader["VILL_CD"]);
                                mc.block_cd = UtilityM.CheckNull<string>(reader["BLOCK_CD"]);
                                mc.service_area_cd = UtilityM.CheckNull<string>(reader["SERVICE_AREA_CD"]);
                                mc.occupation = UtilityM.CheckNull<string>(reader["OCCUPATION"]);
                                mc.phone = UtilityM.CheckNull<string>(reader["PHONE"]);
                                mc.present_address = UtilityM.CheckNull<string>(reader["PRESENT_ADDRESS"]);
                                mc.farmer_type = UtilityM.CheckNull<string>(reader["FARMER_TYPE"]);
                                mc.email = UtilityM.CheckNull<string>(reader["EMAIL"]);
                                mc.monthly_income = UtilityM.CheckNull<decimal>(reader["MONTHLY_INCOME"]);
                                mc.date_of_death = UtilityM.CheckNull<DateTime>(reader["DATE_OF_DEATH"]);
                                mc.sms_flag = UtilityM.CheckNull<string>(reader["SMS_FLAG"]);
                                mc.status = UtilityM.CheckNull<string>(reader["STATUS"]);
                                mc.pan = UtilityM.CheckNull<string>(reader["PAN"]);
                                mc.nominee = UtilityM.CheckNull<string>(reader["NOMINEE"]);
                                mc.nom_relation = UtilityM.CheckNull<string>(reader["NOM_RELATION"]);
                                mc.kyc_photo_type = UtilityM.CheckNull<string>(reader["KYC_PHOTO_TYPE"]);
                                mc.kyc_photo_no = UtilityM.CheckNull<string>(reader["KYC_PHOTO_NO"]);
                                mc.kyc_address_type = UtilityM.CheckNull<string>(reader["KYC_ADDRESS_TYPE"]);
                                mc.kyc_address_no = UtilityM.CheckNull<string>(reader["KYC_ADDRESS_NO"]);
                                mc.org_status = UtilityM.CheckNull<string>(reader["ORG_STATUS"]);
                                mc.org_reg_no = UtilityM.CheckNull<decimal>(reader["ORG_REG_NO"]);
                                mc.created_by = UtilityM.CheckNull<string>(reader["CREATED_BY"]);
                                mc.created_dt = UtilityM.CheckNull<DateTime>(reader["CREATED_DT"]);
                                mc.modified_by = UtilityM.CheckNull<string>(reader["MODIFIED_BY"]);
                                mc.modified_dt = UtilityM.CheckNull<DateTime>(reader["MODIFIED_DT"]);

                                custRets.Add(mc);
                            }
                        }
                    }
                }
            }
            return custRets;
        }
        internal List<mm_customer> GetCustShortDtls(mm_customer pmc)
        {
            List<mm_customer> custRets = new List<mm_customer>();
            string _query = "SELECT  MM_CUSTOMER.BRN_CD,"
         + " MM_CUSTOMER.CUST_CD,"
         + " MM_CUSTOMER.CUST_TYPE,"
         + " MM_CUSTOMER.TITLE,"
         + " MM_CUSTOMER.FIRST_NAME,"
         + " MM_CUSTOMER.MIDDLE_NAME,"
         + " MM_CUSTOMER.LAST_NAME,"
         + " MM_CUSTOMER.CUST_NAME,"
         + " MM_CUSTOMER.GUARDIAN_NAME,"
         + " MM_CUSTOMER.CUST_DT,"
         + " MM_CUSTOMER.OLD_CUST_CD,"
         + " MM_CUSTOMER.DT_OF_BIRTH,"
         + " MM_CUSTOMER.AGE,"
         + " MM_CUSTOMER.SEX,"
         + " MM_CUSTOMER.MARITAL_STATUS,"
         + " MM_CUSTOMER.CATG_CD,"
         + " MM_CUSTOMER.COMMUNITY,"
         + " MM_CUSTOMER.CASTE,"
         + " MM_CUSTOMER.PERMANENT_ADDRESS,"
         + " MM_CUSTOMER.WARD_NO,"
         + " MM_CUSTOMER.STATE,"
         + " MM_CUSTOMER.DIST,"
          + " MM_CUSTOMER.PIN,"
         + " MM_CUSTOMER.VILL_CD,"
         + " MM_CUSTOMER.BLOCK_CD,"
         + " MM_CUSTOMER.SERVICE_AREA_CD,"
         + " MM_CUSTOMER.OCCUPATION,"
         + " MM_CUSTOMER.PHONE,"
         + " MM_CUSTOMER.PRESENT_ADDRESS,"
         + " MM_CUSTOMER.FARMER_TYPE,"
         + " MM_CUSTOMER.EMAIL,"
         + " MM_CUSTOMER.MONTHLY_INCOME,"
         + " MM_CUSTOMER.DATE_OF_DEATH,"
         + " MM_CUSTOMER.SMS_FLAG,"
         + " MM_CUSTOMER.STATUS,"
         + " MM_CUSTOMER.PAN,"
         + " MM_CUSTOMER.NOMINEE,"
         + " MM_CUSTOMER.NOM_RELATION,"
         + " MM_CUSTOMER.KYC_PHOTO_TYPE,"
         + " MM_CUSTOMER.KYC_PHOTO_NO,"
         + " MM_CUSTOMER.KYC_ADDRESS_TYPE,"
         + " MM_CUSTOMER.KYC_ADDRESS_NO,"
         + " MM_CUSTOMER.ORG_STATUS,"
         + " MM_CUSTOMER.ORG_REG_NO,"
         + " MM_CUSTOMER.CREATED_BY,"
         + " MM_CUSTOMER.CREATED_DT,"
         + " MM_CUSTOMER.MODIFIED_BY,"
         + " MM_CUSTOMER.MODIFIED_DT"
    + " FROM  MM_CUSTOMER"
   + " WHERE MM_CUSTOMER.CUST_TYPE='M'"
   + " AND MM_CUSTOMER.BRN_CD={0}"
   + " AND MM_CUSTOMER.CUST_CD={1}";
            using (var connection = OrclDbConnection.NewConnection)
            {

                _statement = string.Format(_query,
                                            !string.IsNullOrWhiteSpace(pmc.brn_cd) ? string.Concat("'", pmc.brn_cd, "'") : "MM_CUSTOMER.BRN_CD",
                                            !string.IsNullOrWhiteSpace(pmc.cust_cd.ToString()) ? string.Concat("'", pmc.cust_cd, "'") : "MM_CUSTOMER.CUST_CD"
                                            );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var mc = new mm_customer();
                                mc.brn_cd = UtilityM.CheckNull<string>(reader["BRN_CD"]);
                                mc.cust_cd = UtilityM.CheckNull<decimal>(reader["CUST_CD"]);
                                mc.cust_type = UtilityM.CheckNull<string>(reader["CUST_TYPE"]);
                                mc.title = UtilityM.CheckNull<string>(reader["TITLE"]);
                                mc.first_name = UtilityM.CheckNull<string>(reader["FIRST_NAME"]);
                                mc.middle_name = UtilityM.CheckNull<string>(reader["MIDDLE_NAME"]);
                                mc.last_name = UtilityM.CheckNull<string>(reader["LAST_NAME"]);
                                mc.cust_name = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                mc.guardian_name = UtilityM.CheckNull<string>(reader["GUARDIAN_NAME"]);
                                mc.cust_dt = UtilityM.CheckNull<DateTime>(reader["CUST_DT"]);
                                mc.old_cust_cd = UtilityM.CheckNull<string>(reader["OLD_CUST_CD"]);
                                mc.dt_of_birth = UtilityM.CheckNull<DateTime>(reader["DT_OF_BIRTH"]);
                                mc.age = UtilityM.CheckNull<decimal>(reader["AGE"]);
                                mc.sex = UtilityM.CheckNull<string>(reader["SEX"]);
                                mc.marital_status = UtilityM.CheckNull<string>(reader["MARITAL_STATUS"]);
                                mc.catg_cd = UtilityM.CheckNull<int>(reader["CATG_CD"]);
                                mc.community = UtilityM.CheckNull<decimal>(reader["COMMUNITY"]);
                                mc.caste = UtilityM.CheckNull<decimal>(reader["CASTE"]);
                                mc.permanent_address = UtilityM.CheckNull<string>(reader["PERMANENT_ADDRESS"]);
                                mc.ward_no = UtilityM.CheckNull<decimal>(reader["WARD_NO"]);
                                mc.state = UtilityM.CheckNull<string>(reader["STATE"]);
                                mc.dist = UtilityM.CheckNull<string>(reader["DIST"]);
                                mc.pin = UtilityM.CheckNull<int>(reader["PIN"]);
                                mc.vill_cd = UtilityM.CheckNull<string>(reader["VILL_CD"]);
                                mc.block_cd = UtilityM.CheckNull<string>(reader["BLOCK_CD"]);
                                mc.service_area_cd = UtilityM.CheckNull<string>(reader["SERVICE_AREA_CD"]);
                                mc.occupation = UtilityM.CheckNull<string>(reader["OCCUPATION"]);
                                mc.phone = UtilityM.CheckNull<string>(reader["PHONE"]);
                                mc.present_address = UtilityM.CheckNull<string>(reader["PRESENT_ADDRESS"]);
                                mc.farmer_type = UtilityM.CheckNull<string>(reader["FARMER_TYPE"]);
                                mc.email = UtilityM.CheckNull<string>(reader["EMAIL"]);
                                mc.monthly_income = UtilityM.CheckNull<decimal>(reader["MONTHLY_INCOME"]);
                                mc.date_of_death = UtilityM.CheckNull<DateTime>(reader["DATE_OF_DEATH"]);
                                mc.sms_flag = UtilityM.CheckNull<string>(reader["SMS_FLAG"]);
                                mc.status = UtilityM.CheckNull<string>(reader["STATUS"]);
                                mc.pan = UtilityM.CheckNull<string>(reader["PAN"]);
                                mc.nominee = UtilityM.CheckNull<string>(reader["NOMINEE"]);
                                mc.nom_relation = UtilityM.CheckNull<string>(reader["NOM_RELATION"]);
                                mc.kyc_photo_type = UtilityM.CheckNull<string>(reader["KYC_PHOTO_TYPE"]);
                                mc.kyc_photo_no = UtilityM.CheckNull<string>(reader["KYC_PHOTO_NO"]);
                                mc.kyc_address_type = UtilityM.CheckNull<string>(reader["KYC_ADDRESS_TYPE"]);
                                mc.kyc_address_no = UtilityM.CheckNull<string>(reader["KYC_ADDRESS_NO"]);
                                mc.org_status = UtilityM.CheckNull<string>(reader["ORG_STATUS"]);
                                mc.org_reg_no = UtilityM.CheckNull<decimal>(reader["ORG_REG_NO"]);
                                mc.created_by = UtilityM.CheckNull<string>(reader["CREATED_BY"]);
                                mc.created_dt = UtilityM.CheckNull<DateTime>(reader["CREATED_DT"]);
                                mc.modified_by = UtilityM.CheckNull<string>(reader["MODIFIED_BY"]);
                                mc.modified_dt = UtilityM.CheckNull<DateTime>(reader["MODIFIED_DT"]);

                                custRets.Add(mc);
                            }
                        }
                    }
                }
            }
            return custRets;
        }

        internal List<tm_deposit> GetDepositDtls(mm_customer pmc)
        {
            List<tm_deposit> depo = new List<tm_deposit>();
            string _query =  " SELECT TM_DEPOSIT.ACC_TYPE_CD, TM_DEPOSIT.ACC_NUM, "
                           + " Decode(TM_DEPOSIT.ACC_TYPE_CD, 1,TM_DEPOSIT.CLR_BAL,7,TM_DEPOSIT.CLR_BAL,8,TM_DEPOSIT.CLR_BAL,6, f_get_rd_prn (TM_DEPOSIT.ACC_NUM,SYSDATE),TM_DEPOSIT.PRN_AMT) Balance, "
                           + " TM_DEPOSIT.CUST_CD,MM_CUSTOMER.CUST_NAME,TM_DEPOSIT.ACC_STATUS  "
                           + " FROM  TM_DEPOSIT,MM_CUSTOMER   "
                           + " WHERE  TM_DEPOSIT.CUST_CD = MM_CUSTOMER.CUST_CD   "
                           + " And    TM_DEPOSIT.CUST_CD = {0}  "
                           + " And    TM_DEPOSIT.BRN_CD = {1}  "
                           + " Order By TM_DEPOSIT.ACC_TYPE_CD  ";
            using (var connection = OrclDbConnection.NewConnection)
            {

                _statement = string.Format(_query,
                                            !string.IsNullOrWhiteSpace(pmc.cust_cd.ToString()) ? string.Concat("'", pmc.cust_cd, "'") : "MM_CUSTOMER.CUST_CD",
                                            !string.IsNullOrWhiteSpace(pmc.brn_cd) ? string.Concat("'", pmc.brn_cd, "'") : "MM_CUSTOMER.BRN_CD"
                                            );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var mc = new tm_deposit();
                                mc.acc_type_cd = UtilityM.CheckNull<int>(reader["ACC_TYPE_CD"]);
                                mc.acc_num = UtilityM.CheckNull<string>(reader["ACC_NUM"]);
                                mc.clr_bal = UtilityM.CheckNull<decimal>(reader["Balance"]);
                                mc.cust_cd = UtilityM.CheckNull<decimal>(reader["CUST_CD"]);
                                mc.created_by = UtilityM.CheckNull<string>(reader["CUST_NAME"]);
                                mc.acc_status = UtilityM.CheckNull<string>(reader["ACC_STATUS"]);
                                depo.Add(mc);
                            }
                        }
                    }
                }
            }
            return depo;
        }

        internal List<tm_loan_all> GetLoanDtls(mm_customer pmc)
        {
            List<tm_loan_all> depo = new List<tm_loan_all>();
            string _query = " SELECT TM_LOAN_ALL.PARTY_CD,TM_LOAN_ALL.LOAN_ID,MM_PARTY.PARTY_NAME, "
+ " Sum(TM_LOAN_ALL.CURR_PRN) CURR_PRN,Sum(TM_LOAN_ALL.OVD_PRN) OVD_PRN, "
+ " Sum(TM_LOAN_ALL.CURR_INTT) CURR_INTT,Sum(TM_LOAN_ALL.OVD_INTT) OVD_INTT, "
+ " Sum(TM_LOAN_ALL.CURR_PRN) + Sum(TM_LOAN_ALL.OVD_PRN) +Sum(TM_LOAN_ALL.CURR_INTT)+ Sum(TM_LOAN_ALL.OVD_INTT) out_standing, "
+ " TM_LOAN_ALL.ACC_CD "
+ " FROM   TM_LOAN_ALL,MM_PARTY "
+ " WHERE  TM_LOAN_ALL.LOAN_ID = TM_LOAN_ALL.LOAN_ID "
+ " And TM_LOAN_ALL.PARTY_CD = MM_PARTY.PARTY_CD "
+ " And TM_LOAN_ALL.PARTY_CD = To_Char({0})"
+ " Group By TM_LOAN_ALL.PARTY_CD,TM_LOAN_ALL.LOAN_ID, "
+ " MM_PARTY.PARTY_NAME,TM_LOAN_ALL.ACC_CD";
            using (var connection = OrclDbConnection.NewConnection)
            {

                _statement = string.Format(_query,
                                            !string.IsNullOrWhiteSpace(pmc.cust_cd.ToString()) ? string.Concat("'", pmc.cust_cd, "'") : "MM_CUSTOMER.CUST_CD"
                                            );
                using (var command = OrclDbConnection.Command(connection, _statement))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var mc = new tm_loan_all();
                                mc.party_cd = UtilityM.CheckNull<decimal>(reader["PARTY_CD"]);
                                mc.loan_id = UtilityM.CheckNull<string>(reader["LOAN_ID"]);
                                mc.created_by = UtilityM.CheckNull<string>(reader["PARTY_NAME"]);
                                mc.curr_prn = UtilityM.CheckNull<decimal>(reader["CURR_PRN"]);
                                mc.ovd_prn = UtilityM.CheckNull<decimal>(reader["OVD_PRN"]);
                                mc.curr_intt = UtilityM.CheckNull<decimal>(reader["CURR_INTT"]);
                                mc.ovd_intt = UtilityM.CheckNull<decimal>(reader["OVD_INTT"]);
                                mc.instl_amt = UtilityM.CheckNull<decimal>(reader["out_standing"]);
                                mc.acc_cd = UtilityM.CheckNull<Int32>(reader["ACC_CD"]);

                                depo.Add(mc);
                            }
                        }
                    }
                }
            }
            return depo;
        }


    }
}