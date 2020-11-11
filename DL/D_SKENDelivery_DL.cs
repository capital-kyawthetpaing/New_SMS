using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using Entity;
using System.Data;

namespace DL
{
   public class D_SKENDelivery_DL: Base_DL
    {
        public DataTable D_SKENDelivery_SelectAll()
        {
            string sp = "D_SKENDelivery_SelectAll";
            
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {

            };

            return SelectData(dic, sp);

        }
        public bool M_MultiPorpose_Update(M_MultiPorpose_Entity mme)
        {
            string sp = "M_MultiPorpose_Update";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ID", new ValuePair { value1 = SqlDbType.Int, value2 = mme.ID } },
                { "@Key", new ValuePair { value1 = SqlDbType.VarChar, value2 = mme.Key } },
                { "@Num1", new ValuePair { value1 = SqlDbType.Int, value2 = mme.Num1 } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mme.UpdateOperator } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = mme.PC } },
            };

            UseTransaction = true;
            return InsertUpdateDeleteData(dic, sp);
        }
        public DataTable D_SKENDeliveryDetails_SelectAll(D_SKENDeliveryDetails_Entity de)
        {
            string sp = "D_SKENDeliveryDetails_SelectAll";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                //{ "@SKENNouhinshoNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.SKENNouhinshoNO } },
                { "@SKENBangouA", new ValuePair { value1 = SqlDbType.Int, value2 = de.SKENBangouA } },
                { "@ChkFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkFlg } },

            };

            return SelectData(dic, sp); 
        }

        public bool SKEN_InsertData(D_SKENDelivery_Entity dskend)
        {
            string sp = "D_SKEN_InsertData";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dskend.VendorCD } },
                { "@xmlSKENDelivery", new ValuePair { value1 = SqlDbType.VarChar, value2 = dskend.xml1 } },
                { "@xmlSKENDeliveryDetails", new ValuePair { value1 = SqlDbType.VarChar, value2 =dskend.xml2 } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = dskend.UpdateOperator } },
            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, sp);
        }
    }
}
