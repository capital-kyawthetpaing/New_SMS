using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;

namespace DL
{
    public class M_API_DL : Base_DL
    {
        public bool M_API_InsertRefreshToken_Yahoo(M_API_Entity api)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
               {
                   { "@APIKey",new ValuePair { value1=SqlDbType.TinyInt,value2= api.APIKey} } ,
                   { "@ChangeDate",new ValuePair { value1=SqlDbType.Date,value2= api.ChangeDate} } ,
                   { "@Token", new ValuePair { value1 = SqlDbType.VarChar, value2 = api.RefreshToken} },
               };
            return InsertUpdateDeleteData(dic, "M_API_InsertRefreshToken_Yahoo");
        }

    }
}
