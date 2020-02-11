using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DL
{
  public class D_WowmaJuchuu_DL : Base_DL
    {
        public DataTable InsertSelect_OrderData(D_WowmaRequest_Entity wowma_e)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {               
                { "@OrderListXml", new ValuePair { value1 = SqlDbType.VarChar, value2 =wowma_e.OrderListXml} },
                { "@APIKey", new ValuePair { value1 = SqlDbType.TinyInt, value2 =wowma_e.APIKey} },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = wowma_e.StoreCD } },
                { "@LastUpdatedBefore", new ValuePair { value1 = SqlDbType.VarChar, value2 = wowma_e.LastUpdatedBefore } },
                { "@LastUpdatedAfter", new ValuePair { value1 = SqlDbType.VarChar, value2 = wowma_e.LastUpdatedAfter } },

            };
            UseTransaction = true;
            return SelectData(dic, "WowmaOrder_InsertSelect");

        }

        public bool Insert_OrderDetailData(Base_Entity be)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@JuChuuXml", new ValuePair { value1 = SqlDbType.VarChar, value2 = be.xml1 } },
                { "@DetailXml", new ValuePair { value1 = SqlDbType.VarChar, value2 = be.xml2 } }
            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, "WowmaDetail_Insert");
            
        }

    }
}
