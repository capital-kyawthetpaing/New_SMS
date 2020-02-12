using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DL
{
    public class D_MonthlyStock_DL : Base_DL
    {

        /// <summary>
        /// 月次債権計算処理
        /// GetsujiZaikoKeisanSyoriより更新時に使用
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public bool D_MonthlyStock_Exec(D_MonthlyStock_Entity de)
        {
            string sp = "PRC_GetsujiZaikoKeisanSyori";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@FiscalYYYYMM", new ValuePair { value1 = SqlDbType.Int, value2 = de.YYYYMM} },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.StoreCD } },
                { "@Mode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.Mode.ToString()} },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.Operator} },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.PC} },
            };

            UseTransaction = true;
            return InsertUpdateDeleteData(dic, sp);
        }
    } 
}
