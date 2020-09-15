using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DL
{
    public class D_MonthlyPurchase_DL : Base_DL
    {

        /// <summary>
        /// 月次仕入計算処理
        /// GetsujiShiireKeisanSyoriより更新時に使用
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public bool D_MonthlyPurchase_Exec(D_MonthlyPurchase_Entity de)
        {
            string sp = "PRC_GetsujiShiireKeisanSyori";

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
        public DataTable RPC_SiiresakiZaikoYoteiHyou(D_MonthlyPurchase_Entity dmpe)
        {
            string sp = "RPC_SiiresakiZaikoYoteiHyou";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                {"@StoreCD",new ValuePair{value1=SqlDbType.VarChar,value2=dmpe.StoreCD} },
                {"@TargetSDate",new ValuePair{value1=SqlDbType.Int,value2=dmpe.YYYYMMS.Replace("-","")} },
                {"@TargetEDate",new ValuePair{value1=SqlDbType.Int,value2=dmpe.YYYYMME.Replace("-","")} }
            };
            return SelectData(dic, sp);
        }
    } 
}
