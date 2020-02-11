using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;

namespace DL
{
    public class D_RakutenRequest_DL:Base_DL
    {
        public DataTable InsertSelect_SearchOrderData(D_RakutenRequest_Entity DRakutenReq_entity)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@OrderListXml", new ValuePair { value1 = SqlDbType.VarChar, value2 = DRakutenReq_entity.OrderListXml} },
                { "@APIKey", new ValuePair { value1 = SqlDbType.TinyInt, value2 = DRakutenReq_entity.APIKey } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = DRakutenReq_entity.StoreCD } },
                { "@LastUpdatedBefore", new ValuePair { value1 = SqlDbType.VarChar, value2 = DRakutenReq_entity.LastUpdatedBefore } },
                { "@LastUpdatedAfter", new ValuePair { value1 = SqlDbType.VarChar, value2 = DRakutenReq_entity.LastUpdatedAfter } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = DRakutenReq_entity.InsertOperator } },

            };
            UseTransaction = true;
            return SelectData(dic, "RakutenOrder_InsertSelect");

        }
    }
}
