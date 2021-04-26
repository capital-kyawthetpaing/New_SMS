using System.Collections.Generic;
using Entity;
using System.Data;

namespace DL
{
    public class M_CustomerInitial_DL : Base_DL
    {
        /// <summary>	
        /// </summary>	
        /// <param name="me"></param>	
        /// <returns></returns>
        public DataTable M_CustomerInitial_Select(M_CustomerInitial_Entity me)
        {
            string sp = "M_CustomerInitial_Select";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.StoreKBN } },
                //{ "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.ChangeDate } }
            };

            return SelectData(dic, sp);
        }
        }
}
