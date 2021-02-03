using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
   public class MasterTorikomi_SKU_DL:Base_DL
    {
        public Boolean MasterTorikomi_SKU_Insert_Update(int type, M_SKU_Entity mE)
        {
            Dictionary<String, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@type", new ValuePair { value1 = SqlDbType.Int, value2 =type.ToString()  } },
                { "@xml", new ValuePair { value1 = SqlDbType.Xml, value2 = mE.xml1 } },
                { "@OperatorCD",new ValuePair { value1 = SqlDbType.VarChar, value2 = mE.Operator} },
                { "@ProgramID",new ValuePair { value1 = SqlDbType.VarChar, value2 = mE.ProgramID} },
                { "@PC",new ValuePair { value1 = SqlDbType.VarChar, value2 = mE.PC} },
                { "@KeyItem",new ValuePair { value1 = SqlDbType.VarChar, value2 = mE.Key} }
               
            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, "MasterTorikomi_SKU_Insert_Update");
        }

        public DataTable M_SKUInitial_SelectAll()
        {
            Dictionary<String, ValuePair> dic = new Dictionary<string, ValuePair>
            {};
            
            return SelectData(dic, "M_SKUInitial_SelectAll");
        }
    }
}
