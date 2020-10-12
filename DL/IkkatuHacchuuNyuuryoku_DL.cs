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
    public class IkkatuHacchuuNyuuryoku_DL : Base_DL
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hacchuuDate"></param>
        /// <param name="vendorCD"></param>
        /// <param name="storeCD"></param>
        /// <returns></returns>
        public DataTable PRC_IkkatuHacchuuNyuuryoku_SelectData(string ikkatuHacchuuMode, string hacchuuDate, string vendorCD,string juchuuStaffCD, string storeCD, string isSaiHacchuu)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@p_IkkatuHacchuuMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = ikkatuHacchuuMode } },
                { "@p_HacchuuDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = hacchuuDate } },
                { "@p_VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = vendorCD } },
                { "@p_JuchuuStaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = juchuuStaffCD } },
                { "@p_StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = storeCD } },
                { "@p_IsSaiHacchuu", new ValuePair { value1 = SqlDbType.VarChar, value2 = isSaiHacchuu } },
            };
            return SelectData(dic, "PRC_IkkatuHacchuuNyuuryoku_SelectData");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderNO"></param>
        /// <returns></returns>
        public DataTable PRC_IkkatuHacchuuNyuuryoku_SelectByOrderNO(string orderNO,string orderProcessNO)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@p_OrderNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = orderNO } },
                { "@p_OrderProcessNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = orderProcessNO } },
            };
            return SelectData(dic, "PRC_IkkatuHacchuuNyuuryoku_SelectByOrderNO");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationMode"></param>
        /// <param name="operatorID"></param>
        /// <param name="pc"></param>
        /// <param name="storeCD"></param>
        /// <param name="staffCD"></param>
        /// <param name="hacchuuDate"></param>
        /// <param name="dtTIkkatuHacchuuNyuuryoku"></param>
        /// <returns></returns>
        public bool PRC_IkkatuHacchuuNyuuryoku_Register(int operationMode,string operatorID,string pc,string storeCD,string staffCD, string orderDate, string orderNO, string orderProcessNO, string ikkatuHacchuuMode, DataTable dtTIkkatuHacchuuNyuuryoku)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@p_OperateMode", new ValuePair { value1 = SqlDbType.Int, value2 = operationMode.ToString() } },
                { "@p_Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = operatorID } },
                { "@p_PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = pc} },
                { "@p_StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = storeCD} },
                { "@p_StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = staffCD} },
                { "@p_OrderDate", new ValuePair { value1 = SqlDbType.Date, value2 = orderDate} },
                { "@p_OrderNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = orderNO} },
                { "@p_IkkatuHacchuuMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = ikkatuHacchuuMode} },
                { "@p_OrderProcessNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = orderProcessNO} },
            };
            bool ret = true;
            string sp = "PRC_IkkatuHacchuuNyuuryoku_Register";
            try
            {
                StartTransaction();
                command = new SqlCommand(sp, GetConnection(), transaction);
                command.CommandType = CommandType.StoredProcedure;
                foreach (KeyValuePair<string, ValuePair> pair in dic)
                {
                    ValuePair vp = pair.Value;
                    AddParam(command, pair.Key, vp.value1, vp.value2);
                }
                SqlParameter p = command.Parameters.AddWithValue("@p_TIkkatuHacchuuNyuuryoku", dtTIkkatuHacchuuNyuuryoku);
                p.SqlDbType = SqlDbType.Structured;

                command.ExecuteNonQuery();

                CommitTransaction();

                ret = true;
            }
            catch (Exception e)
            {
                RollBackTransaction();
                ret = false;
                throw e;
            }
            finally
            {
                command.Connection.Close();
            }
            return ret;
        }
        public DataTable D_Order_SelectData_SeachHacchuuShoriNO(string storeCD, string dateFrom, string dateTo, string staffCD)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
               {"@p_StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = storeCD } },
               {"@p_DateFrom", new ValuePair { value1 = SqlDbType.Date, value2 = dateFrom } },
               {"@p_DateTo", new ValuePair { value1 = SqlDbType.Date, value2 = dateTo } },
               {"@p_InsertOperator", new ValuePair { value1 = SqlDbType.VarChar, value2 = staffCD } },
            };
            UseTransaction = true;
            return SelectData(dic, "D_Order_SelectData_SeachHacchuuShoriNO");
        }
    }
}
