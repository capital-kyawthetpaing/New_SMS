using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DL
{
    public class M_Store_DL : Base_DL
    {
        /// <summary>
        /// Select Store's info
        /// </summary>
        /// <param name="mse">staff info</param>
        /// <returns></returns>
        public DataTable M_Store_Bind(M_Store_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.StoreCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mse.ChangeDate } },
                { "@Type", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.Type } },
                { "@DeleteFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.DeleteFlg } }
                
            };
            return SelectData(dic, "M_Store_Bind");
        }

        /// <summary>
        /// Select Store's info
        /// StoreKBN IN (1,3)のStore情報をBind
        /// </summary>
        /// <param name="mse">staff info</param>
        /// <returns></returns>
        public DataTable M_Store_Bind_Mitsumori(M_Store_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.StoreCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mse.ChangeDate } },
                { "@DeleteFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.DeleteFlg } }
            };
            return SelectData(dic, "M_Store_Bind_Mitsumori");
        }
        /// <summary>
        /// Select Store's info
        /// StoreKBN IN 1のStore情報をBind
        /// </summary>
        /// <param name="mse">staff info</param>
        /// <returns></returns>
        public DataTable M_Store_Bind_Juchu(M_Store_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.StoreCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mse.ChangeDate } },
                { "@DeleteFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.DeleteFlg } }
            };
            return SelectData(dic, "M_Store_Bind_Juchu");
        }

        /// <summary>
        /// Select Store's info
        /// StoreKBN NOT IN 2のStore情報をBind（権限のある店舗のみ）
        /// </summary>
        /// <param name="mse">staff info</param>
        /// <returns></returns>
        public DataTable M_Store_Bind_Getsuji(M_Store_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.StoreCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mse.ChangeDate } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.Operator } },
                { "@DeleteFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.DeleteFlg } }
            };
            return SelectData(dic, "M_Store_Bind_Getsuji");
        }

        public DataTable M_Store_Select(M_Store_Entity mbe)
        {
            string sp = "M_Store_Select";

            //command = new SqlCommand(sp, GetConnection());
            //command.CommandType = CommandType.StoredProcedure;
            //command.CommandTimeout = 0;

            //command.Parameters.Add("@StoreCD", SqlDbType.VarChar).Value = mbe.StoreCD;
            //command.Parameters.Add("@ChangeDate", SqlDbType.VarChar).Value = mbe.ChangeDate;

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.StoreCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mbe.ChangeDate } },
            };
            return SelectData(dic, sp);

            //return SelectData(sp);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mse"></param>
        /// <returns></returns>
        public DataTable M_Store_InitSelect(M_Staff_Entity mse)
        {
            string sp = "M_Store_InitSelect";

            //KTP 2019-0529 全部のFunctionでしなくてもいいように共通のFunctionでやり方を更新しました。
            //command = new SqlCommand(sp, GetConnection());
            //command.CommandType = CommandType.StoredProcedure;
            //command.CommandTimeout = 0;

            //command.Parameters.Add("@StaffCD", SqlDbType.VarChar).Value = mse.StaffCD;
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.StaffCD } }
            };

            //return SelectData(sp);
            return SelectData(dic, sp);
        }
        public DataTable M_Store_SelectAll(M_Store_Entity mbe)
        {
            string sp = "M_Store_SelectAll";

            //KTP 2019-0529 全部のFunctionでをしなくてもいいように共通のFunctionでやり方を更新しました。
            //command = new SqlCommand(sp, GetConnection());
            //command.CommandType = CommandType.StoredProcedure;
            //command.CommandTimeout = 0;

            //command.Parameters.Add("@DisplayKbn", SqlDbType.TinyInt).Value = mbe.DisplayKbn;
            //command.Parameters.Add("@ChangeDate", SqlDbType.VarChar).Value = mbe.ChangeDate;
            //command.Parameters.Add("@StoreCDFrom", SqlDbType.VarChar).Value = mbe.StoreCDFrom;
            //command.Parameters.Add("@StoreCDTo", SqlDbType.VarChar).Value = mbe.StoreCDTo;
            //command.Parameters.Add("@StoreName", SqlDbType.VarChar).Value = mbe.StoreName;
            //command.Parameters.Add("@StoreKBN1", SqlDbType.TinyInt).Value = mbe.StoreKBN1;
            //command.Parameters.Add("@StoreKBN2", SqlDbType.TinyInt).Value = mbe.StoreKBN2;
            //command.Parameters.Add("@StoreKBN3", SqlDbType.TinyInt).Value = mbe.StoreKBN3;
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@DisplayKbn", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mbe.DisplayKbn } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.ChangeDate } },
                { "@StoreCDFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.StoreCDFrom } },
                { "@StoreCDTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.StoreCDTo } },
                { "@StoreName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.StoreName } },
                { "@StoreKBN1", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mbe.StoreKBN1 } },
                { "@StoreKBN2", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mbe.StoreKBN2 } },
                { "@StoreKBN3", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mbe.StoreKBN3 } },
            };

            //return SelectData(sp);
            return SelectData(dic, sp);
        }

        /// <summary>	
        /// 店舗マスタ更新処理	
        /// MasterTouroku_Tempoより更新時に使用	
        /// </summary>	
        /// <param name="mbe"></param>	
        /// <param name="operationMode"></param>	
        /// <param name="operatorNm"></param>	
        /// <param name="pc"></param>	
        /// <returns></returns>	
        public bool M_Store_Exec(M_Store_Entity mbe, short operationMode, string operatorNm, string pc)
        {
            string sp = "PRC_MasterTouroku_Tempo";

            //KTP 2019-06-03 add Datatype to param	
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = operationMode.ToString() } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.StoreCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mbe.ChangeDate} },
                { "@StoreName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.StoreName} },
                { "@StoreKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mbe.StoreKBN} },
                { "@StorePlaceKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mbe.StorePlaceKBN} },
                { "@MallCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.MallCD} },
                { "@APIKey", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mbe.APIKey} },
                { "@ZipCD1", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.ZipCD1} },
                { "@ZipCD2", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.ZipCD2} },
                { "@Address1", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.Address1} },
                { "@Address2", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.Address2} },
                { "@TelphoneNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.TelphoneNO} },
                { "@FaxNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.FaxNO} },
                { "@MailAddress1", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.MailAddress1} },	
                //{ "@MailAddress2", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.MailAddress2} },	
                //{ "@MailAddress3", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.MailAddress3} },	
                { "@ApprovalStaffCD11", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.ApprovalStaffCD11} },
                { "@ApprovalStaffCD12", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.ApprovalStaffCD12} },
                { "@ApprovalStaffCD21", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.ApprovalStaffCD21} },
                { "@ApprovalStaffCD22", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.ApprovalStaffCD22} },
                { "@ApprovalStaffCD31", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.ApprovalStaffCD31} },
                { "@ApprovalStaffCD32", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.ApprovalStaffCD32} },
                { "@DeliveryDate",  new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.DeliveryDate} },
                { "@PaymentTerms",  new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.PaymentTerms} },
                { "@DeliveryPlace",  new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.DeliveryPlace} },
                { "@ValidityPeriod",  new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.ValidityPeriod} },
                { "@Print1_NoTrim", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.Print1} },
                { "@Print2_NoTrim", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.Print2} },
                { "@Print3_NoTrim", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.Print3} },
                { "@Print4_NoTrim", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.Print4} },
                { "@Print5_NoTrim", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.Print5} },
                { "@Print6_NoTrim", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.Print6} },
                { "@KouzaCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.KouzaCD} },
                { "@ReceiptPrint", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.ReceiptPrint} },
                { "@MoveMailPatternCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.MoveMailPatternCD} },
                { "@InvoiceNotation", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.InvoiceNotation} },
                { "@Remarks", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.Remarks} },
                { "@DeleteFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mbe.DeleteFlg} },
                { "@UsedFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mbe.UsedFlg} },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = operatorNm} },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = pc} },
            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, sp);
        }

        /// <summary>
        /// 店舗マスタ取得処理
        /// MasterTouroku_Tempoよりデータ抽出時に使用
        /// </summary>
        public DataTable M_Store_SelectData(M_Store_Entity mbe)
        {
            string sp = "M_Store_SelectData";
            //KTP 2019-0529 全部のFunctionでをしなくてもいいように共通のFunctionでやり方を更新しました。
            //command = new SqlCommand(sp, GetConnection());
            //command.CommandType = CommandType.StoredProcedure;
            //command.CommandTimeout = 0;

            //command.Parameters.Add("@StoreCD", SqlDbType.VarChar).Value = mbe.StoreCD;
            //command.Parameters.Add("@ChangeDate", SqlDbType.VarChar).Value = mbe.ChangeDate;
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.StoreCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.ChangeDate } }
            };

            //return SelectData(sp);
            return SelectData(dic, sp);
        }

        public DataTable M_Store_BindData(M_Store_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                {"@Operator", new ValuePair {value1 = SqlDbType.VarChar,value2 = ""} }
            };
            return SelectData(dic, "M_Store_BindData");
        }
        public DataTable GetApprovalData(string operatorNm, string storeCD)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                {"@StoreCD", new ValuePair { value1 = SqlDbType.VarChar,value2 = storeCD} },
                {"@Operator", new ValuePair {value1 = SqlDbType.VarChar,value2 = operatorNm} }
            };
            return SelectData(dic, "SelectApprovalData");
        }

        public DataTable GetHonsha(M_Store_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ChangeDate } }
            };

            return SelectData(dic, "SelectHonshaStore");
        }

        /// Select Store's info	
        /// StoreKBN,StorePlaceKBNの条件でStore情報を取得	
        /// </summary>	
        /// <param name="mse">store info</param>	
        /// <returns></returns>	
        public DataTable M_Store_SelectByKbn(M_Store_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.StoreKBN } },
                { "@StorePlaceKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.StorePlaceKBN } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mse.ChangeDate } },
                { "@DeleteFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.DeleteFlg } }
            };
            return SelectData(dic, "M_Store_SelectByKbn");
        }
        public DataTable M_Store_SelectByApiKey(M_Store_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.StoreCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ChangeDate } },
                { "@APIKey", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.APIKey } }
            };
            return SelectData(dic, "M_Store_SelectByApiKey");
        }

    }
}
