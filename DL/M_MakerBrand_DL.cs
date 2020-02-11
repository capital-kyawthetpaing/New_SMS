using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DL
{
    public class M_MakerBrand_DL : Base_DL
    {
    
         /// <summary>
        /// 
        /// </summary>
        /// <param name="mme"></param>
        /// <returns></returns>
        public DataTable M_MakerBrand_Select(M_MakerBrand_Entity mme)
        {
            string sp = "M_MakerBrand_Select";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@BrandCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mme.BrandCD } },
                { "@MakerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mme.MakerCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mme.ChangeDate } }
            };
            
            return SelectData(dic,sp);
        }

        public DataTable M_MakerBrand_SelectAll(M_MakerBrand_Entity mme)
        {
            string sp = "M_MakerBrand_SelectAll";

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
