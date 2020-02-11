using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;

namespace DL
{
   public class M_Payee_DL:Base_DL
    {
        public DataTable D_Payee_PayeeNameSelect(D_Pay_Entity dpe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                {"@PayeeCD", new ValuePair {value1 = SqlDbType.VarChar,value2 = dpe.PayeeCD} },
                {"@PayDate", new ValuePair {value1 = SqlDbType.Date,value2 = dpe.PayDate} }
            };
            return SelectData(dic,"D_Payee_PayeeNameSelect");
        }
    }
}
