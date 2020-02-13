using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;

namespace DL
{
    public class D_MonthlyDebt_DL : Base_DL
    {
        public DataTable D_MonthlyDebt_CSV_Report(D_MonthlyDebt_Entity mde, int chk)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                { "@ChangeDate",new ValuePair{ value1=SqlDbType.VarChar,value2=mde.ChangeDate} },
                { "@YYYYMM",new ValuePair{value1=SqlDbType.VarChar,value2=mde.YYYYMM}},
                { "@StoreCD",new ValuePair{value1=SqlDbType.VarChar,value2=mde.StoreCD} },
                { "@PayeeCD",new ValuePair{value1=SqlDbType.VarChar,value2=mde.PayeeCD} },
                { "@chk",new ValuePair{value1=SqlDbType.VarChar,value2= chk.ToString()} }
            };
            return SelectData(dic, "D_MonthlyDebt_CSV_Report");
        }

        /// <summary>
        /// 月次債務計算処理
        /// GetsujiSaimuKeisanSyoriより更新時に使用
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public bool D_MonthlyDebt_Exec(D_MonthlyDebt_Entity de)
        {
            string sp = "PRC_GetsujiSaimuKeisanSyori";

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
