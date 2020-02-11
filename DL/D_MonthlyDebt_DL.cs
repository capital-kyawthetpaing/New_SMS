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
    }
}
