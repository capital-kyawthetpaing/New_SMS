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
    public class M_Carrier_DL : Base_DL
    {
        //public DataTable M_Carrier_Select(M_Carrier_Entity mke)
        //{
        //    string sp = "M_Carrier_Select";
        //    Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
        //    {
        //        { "@CarrierCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mke.CarrierCD } },
        //        { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mke.ChangeDate } }
        //    };
            
        //    return SelectData(dic, sp);
        //}
        public DataTable M_Carrier_Bind(M_Carrier_Entity mke)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mke.ChangeDate } },
                { "@DeleteFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mke.DeleteFlg } },
            };
            return SelectData(dic, "M_Carrier_Bind");
        }

        /// <summary>
        /// 出荷入力　代引きチェック
        /// </summary>
        /// <param name="mse"></param>
        /// <returns></returns>
        public DataTable M_Carrier_SelectForShukka(M_Carrier_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@CarrierCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.CarrierCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mse.ChangeDate } }
            };
            return SelectData(dic, "M_Carrier_SelectForShukka");
        }
    }
}
