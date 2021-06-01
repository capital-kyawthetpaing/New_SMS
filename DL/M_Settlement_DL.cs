using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DL
{
    public class M_Settlement_DL : Base_DL
    {

        public DataTable M_Settlement_Bind(M_Settlement_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@FileKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 =  mse.FileKBN} },
            };
            return SelectData(dic, "M_Settlement_Bind");
        }
    }
}
