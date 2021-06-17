using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DL
{
    public class Hacchuusho_DL : Base_DL
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hacchuuDate"></param>
        /// <param name="vendorCD"></param>
        /// <param name="storeCD"></param>
        /// <returns></returns>
        public DataTable PRC_Hacchuusho_D_Order_SelectByKey(string orderNO)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@p_OrderNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = orderNO} },
            };
            return SelectData(dic, "PRC_Hacchuusho_D_Order_SelectByKey");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hacchuuDate"></param>
        /// <param name="vendorCD"></param>
        /// <param name="storeCD"></param>
        /// <returns></returns>
        public DataTable PRC_Hacchuusho_M_AutorisationCheck(string AuthorizationsCD, string ChangeDate, string ProgramID, string StoreCD)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@p_AuthorizationsCD",new ValuePair { value1 = SqlDbType.VarChar, value2 = AuthorizationsCD} },
                { "@p_ChangeDate",new ValuePair { value1 = SqlDbType.VarChar, value2 = ChangeDate} },
                { "@p_ProgramID",new ValuePair { value1 = SqlDbType.VarChar, value2 = ProgramID} },
                { "@p_StoreCD",new ValuePair { value1 = SqlDbType.VarChar, value2 = StoreCD} },
            };
            return SelectData(dic, "PRC_Hacchuusho_M_AutorisationCheck");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="hacchuuDate"></param>
        /// <param name="vendorCD"></param>
        /// <param name="storeCD"></param>
        /// <returns></returns>
        public DataTable PRC_Hacchuusho_SelectData(string StoreCD
                                                  , bool? InsatuTaishou_Mihakkou
                                                  , bool? InsatuTaishou_Saihakkou
                                                  , string HacchuuDateFrom
                                                  , string HacchuuDateTo
                                                  , string Vendor
                                                  , string Staff
                                                  , string HacchuuNO
                                                  , bool? IsPrintMisshounin
                                                  , bool? IsPrintEDIHacchuu
                                                  , bool? InsatuShurui_Hacchhusho
                                                  , bool? InsatuShurui_NetHacchuu
                                                  , bool? InsatuShurui_Chokusou
                                                  , bool? InsatuShurui_Cancel)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@p_StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = StoreCD} },
                { "@p_InsatuTaishou_Mihakkou", new ValuePair { value1 = SqlDbType.Bit, value2 = InsatuTaishou_Mihakkou.ToString()} },
                { "@p_InsatuTaishou_Saihakkou", new ValuePair { value1 = SqlDbType.VarChar, value2 = InsatuTaishou_Saihakkou.ToString()} },
                { "@p_HacchuuDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = HacchuuDateFrom} },
                { "@p_HacchuuDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = HacchuuDateTo} },
                { "@p_Vendor", new ValuePair { value1 = SqlDbType.VarChar, value2 = Vendor} },
                { "@p_Staff", new ValuePair { value1 = SqlDbType.VarChar, value2 = Staff} },
                { "@p_HacchuuNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = HacchuuNO} },
                { "@p_IsPrintMisshounin", new ValuePair { value1 = SqlDbType.Bit, value2 = IsPrintMisshounin.ToString()} },
                { "@p_IsPrintEDIHacchuu", new ValuePair { value1 = SqlDbType.Bit, value2 = IsPrintEDIHacchuu.ToString()} },
                { "@p_InsatuShurui_Hacchhusho", new ValuePair { value1 = SqlDbType.Bit, value2 = InsatuShurui_Hacchhusho.ToString()} },
                { "@p_InsatuShurui_NetHacchuu", new ValuePair { value1 = SqlDbType.Bit, value2 = InsatuShurui_NetHacchuu.ToString()} },
                { "@p_InsatuShurui_Chokusou", new ValuePair { value1 = SqlDbType.Bit, value2 = InsatuShurui_Chokusou.ToString()} },
                { "@p_InsatuShurui_Cancel", new ValuePair { value1 = SqlDbType.Bit, value2 = InsatuShurui_Cancel.ToString()} },
            };
            return SelectData(dic, "PRC_Hacchuusho_SelectData");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderNO"></param>
        /// <returns></returns>
        public DataTable PRC_Hacchuusho_SelectByOrderNO(string orderNO,string orderProcessNO)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@p_OrderNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = orderNO } },
                { "@p_OrderProcessNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = orderProcessNO } },
            };
            return SelectData(dic, "PRC_Hacchuusho_SelectByOrderNO");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operatorID"></param>
        /// <param name="storeCD"></param>
        /// <param name="orderCD"></param>
        /// <returns></returns>
        public bool PRC_Hacchuusho_Register(string operatorID,string storeCD,string orderCD)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@p_Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = operatorID } },
                { "@p_StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = storeCD} },
                { "@p_OrderCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = orderCD} },
            };
            return InsertUpdateDeleteData(dic, "PRC_Hacchuusho_Register");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operatorID"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool PRC_Hacchuusho_UpdatePrintDate(string operatorID, DataTable dt)
        {
            string sp = "PRC_Hacchuusho_UpdatePrintDate";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParam(command, "@p_Operator", SqlDbType.VarChar, operatorID);
            AddParamForDataTable(command, "@p_tblHacchusho", SqlDbType.Structured, dt);

            UseTransaction = true;

            string outPutParam = "";    //未使用
            bool ret = InsertUpdateDeleteData(sp, ref outPutParam);

            return ret;
        }
    }
}
