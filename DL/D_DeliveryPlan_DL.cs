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
    public class D_DeliveryPlan_DL : Base_DL
    {
        /// <summary>
        /// 出荷指示登録（第２画面）
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable D_DeliveryPlan_SelectData(D_DeliveryPlan_Entity de)
        {
            string sp = "D_DeliveryPlan_SelectData";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@DeliveryPlanNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.DeliveryPlanNO } },
            };

            return SelectData(dic, sp);
        }
    }
}
