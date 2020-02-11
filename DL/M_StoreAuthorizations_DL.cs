using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;

namespace DL
{
    public class M_StoreAuthorizations_DL : Base_DL
    {
        public DataTable M_StoreAuthorizations_Select(M_StoreAuthorizations_Entity msae)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@AuthorizationsCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = msae.StoreAuthorizationsCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = msae.ChangeDate } },
                //{ "@ProgramID", new ValuePair { value1 = SqlDbType.TinyInt, value2 = msae.ProgramID } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = msae.StoreCD } }
            };
            return SelectData(dic, "M_StoreAuthorization_Select");
        }



    }
}
