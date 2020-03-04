using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DL
{
    public class M_Brand_DL : Base_DL
    {
        public DataTable M_Brand_Select(M_Brand_Entity mme)
        {
            string sp = "M_Brand_Select";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@BrandCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mme.BrandCD } },

            };

            return SelectData(dic, sp);
        }
        public DataTable M_Brand_SelectAll(M_Brand_Entity mme)
        {
            string sp = "M_Brand_SelectAll";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@DisplayKbn", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mme.DisplayKbn } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mme.ChangeDate } },
                { "@BrandName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mme.BrandName } },
                { "@MakerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mme.MakerCD } },

            };
            
            return SelectData(dic, sp);
        }
    }
}
