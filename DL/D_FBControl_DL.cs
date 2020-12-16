using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;
using System.Data.SqlClient;

namespace DL
{
    public class D_FBControl_DL:Base_DL
    {
        public bool D_FBControl_Insert(D_FBControl_Entity dfe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@PayDate", new ValuePair { value1 = SqlDbType.Date, value2 = dfe.PayDate } },
                { "@ActualPayDate", new ValuePair { value1 = SqlDbType.Date, value2 = dfe.ActualPayDate } },
                { "@MotoKouzaCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dfe.MotoKouzaCD } },
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dfe.StaffCD } },
            };

            return InsertUpdateDeleteData(dic, "D_FBControl_Insert");
        }
    }
}
