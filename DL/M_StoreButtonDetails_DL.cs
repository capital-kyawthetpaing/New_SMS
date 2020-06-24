using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DL
{
    public class M_StoreButtonDetails_DL : Base_DL
    {
        public DataTable M_StoreButtonDetails_SelectAll(M_StoreBottunDetails_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                 { "@StoreCD",new ValuePair {value1=SqlDbType.VarChar,value2=mse.StoreCD} },
                 { "@ProgramKBN",new ValuePair {value1=SqlDbType.TinyInt,value2=mse.ProgramKBN} },
                 { "@GroupNO",new ValuePair {value1=SqlDbType.TinyInt,value2=mse.GroupNO} }

            };

            return SelectData(dic, "M_StoreButtonDetails_SelectAll");
        }
        public bool Button_Details_Insert_Update(M_StoreBottunDetails_Entity msbd)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                 { "@GroupXML",new ValuePair {value1=SqlDbType.VarChar,value2=msbd.GroupXML} },
                 { "@GroupDetailXML",new ValuePair {value1=SqlDbType.VarChar,value2=msbd.GroupDetailXML} },
                 {"@StoreCD" ,new ValuePair {value1=SqlDbType.VarChar,value2=msbd.StoreCD}},
                 { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = msbd.Operator } },
                 { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = msbd.ProgramID } },
                 { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = msbd.PC } }
            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, "Button_Details_Insert_Update");
         }
    }
}
