using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

using System.Data.SqlClient;

namespace DL
{
    public class D_Billing_DL : Base_DL
    {
        /// <summary>
        /// 請求照会よりデータ抽出時に使用
        /// </summary>
        /// <param name="dbe"></param>
        /// <returns></returns>
        public DataTable D_Billing_SelectAll(D_Billing_Entity dbe, M_Customer_Entity mce)
        {
            string sp = "D_Billing_SelectAll";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dbe.StoreCD } },
                { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dbe.BillingCustomerCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = dbe.BillingCloseDate } },
                { "@BillingCloseDate", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mce.BillingCloseDate } }
            };
            return SelectData(dic, sp);
        }

        /// <summary>
        /// 店舗請求書よりデータ抽出時に使用
        /// </summary>
        /// <param name="dbe"></param>
        /// <returns></returns>
        public DataTable D_Billing_SelectForPrint(D_Billing_Entity dbe, M_Customer_Entity mce)
        {
            string sp = "D_Billing_SelectForPrint";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@BillingCloseDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = dbe.BillingCloseDate } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dbe.StoreCD } },
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mce.StaffCD } },
                { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dbe.BillingCustomerCD } },
                { "@CustomerName", new ValuePair { value1 = SqlDbType.VarChar, value2 = dbe.CustomerName} },
                { "@PrintFLG", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dbe.PrintFLG } },
            };

            return SelectData(dic, sp);
        }
        /// <summary>
        /// 請求書より更新時に使用
        /// </summary>
        /// <param name="dbe"></param>
        /// <param name="dt"></param>
        /// <param name="operatorNm"></param>
        /// <param name="pc"></param>
        /// <returns></returns>
        public bool D_Billing_Update(D_Billing_Entity dbe, DataTable dt, string operatorNm, string pc)
        {
            string sp = "D_Billing_Update";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParamForDataTable(command, "@Table", SqlDbType.Structured, dt);
            AddParam(command, "@Operator", SqlDbType.VarChar, operatorNm);
            AddParam(command, "@PC", SqlDbType.VarChar, pc);

            UseTransaction = true;

            string outPutParam = "";    //未使用
            bool ret = InsertUpdateDeleteData(sp, ref outPutParam);

            return ret;
        }

        /// <summary>
        /// 入金元検索よりデータ抽出時に使用
        /// </summary>
        /// <param name="dbe"></param>
        /// <returns></returns>
        public DataTable D_Billing_SelectForSearch(D_Billing_Entity dbe)
        {
            string sp = "D_Billing_SelectForSearch";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dbe.StoreCD } },
                { "@CustomerName", new ValuePair { value1 = SqlDbType.VarChar, value2 = dbe.CustomerName } },
                { "@BillingGakuFrom", new ValuePair { value1 = SqlDbType.Money, value2 = dbe.BillingGakuFrom } },
                { "@BillingGakuTo", new ValuePair { value1 = SqlDbType.Money, value2 = dbe.BillingGakuTo } },
                { "@CollectDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dbe.CollectDateFrom } },
                { "@CollectDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dbe.CollectDateTo } },
                { "@ChkMinyukin", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dbe.ChkMinyukin } },
                { "@ChkNyukinzumi", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dbe.ChkNyukinzumi } },
                { "@RdoDispKbn", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dbe.RdoDispKbn.ToString() } },
            };
            return SelectData(dic, sp);
        }
    }
}
