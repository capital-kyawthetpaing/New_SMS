using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;

namespace DL
{
   public class D_RakutenJuchuu_DL:Base_DL
    {
        public bool Insert_GetOrderData(Base_Entity base_entity)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@JuChuuXml", new ValuePair { value1 = SqlDbType.VarChar, value2 = base_entity.xml1} },
                { "@ShippingXml", new ValuePair { value1 = SqlDbType.VarChar, value2 = base_entity.xml2} },
                { "@CouponXml", new ValuePair { value1 = SqlDbType.VarChar, value2 = base_entity.xml3} },
                { "@ChangeReasonXml", new ValuePair { value1 = SqlDbType.VarChar, value2 = base_entity.xml4} },
                { "@JuChuuDetailXml", new ValuePair { value1 = SqlDbType.VarChar, value2 = base_entity.xml5} },
                { "@ShippingDetailXml", new ValuePair { value1 = SqlDbType.VarChar, value2 = base_entity.xml6} },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = base_entity.InsertOperator } },

            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, "RakutenOrderDataDetail_Insert");

        }
    }
}
