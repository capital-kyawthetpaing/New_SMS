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
    public class D_BillingProcessing_DL : Base_DL
    {
        /// <summary>
        /// 請求締処理よりデータ抽出時に使用
        /// </summary>
        /// <param name="dme"></param>
        /// <returns></returns>
        public DataTable D_BillingProcessing_SelectAll(D_BillingProcessing_Entity dme)
        {
            string sp = "D_BillingProcessing_SelectAll";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.StoreCD } },
            };
            return SelectData(dic, sp);
        }

        /// <summary>
        /// 請求締処理
        /// SeikyuuShimeShoriより更新時に使用
        /// </summary>
        /// <param name = "dse" ></ param >
        /// < param name="operationMode"></param>
        /// <param name = "operatorNm" ></ param >
        /// < param name="pc"></param>
        /// <returns></returns>
        public bool D_BillingProcessing_Exec(D_CollectPlan_Entity dce, string operatorNm, string pc)
        {
            string sp = "PRC_SeikyuuShimeShori";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@Syori", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dce.Syori } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.StoreCD } },
                { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.CustomerCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.BillingDate } },
                { "@BillingCloseDate", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dce.BillingCloseDate } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = operatorNm} },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = pc} },
            };

            UseTransaction = true;
            return InsertUpdateDeleteData(dic, sp);
        }

        /// <summary>
        /// 締処理済の場合（以下のSelectができる場合）Error
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable CheckBillingDate(D_BillingProcessing_Entity de)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                {"@StoreCD", new ValuePair {value1 = SqlDbType.VarChar,value2 = de.StoreCD} }  ,
                {"@CustomerCD", new ValuePair {value1 = SqlDbType.VarChar,value2 = de.CustomerCD} }  ,
                {"@BillingDate", new ValuePair {value1 = SqlDbType.VarChar,value2 = de.BillingDate} }


            };
            return SelectData(dic, "CheckBillingDate");
        }
    }
}
