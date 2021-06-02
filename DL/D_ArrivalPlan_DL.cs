using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;

namespace DL
{
   public class D_ArrivalPlan_DL:Base_DL
    {
        public DataTable D_ArrivalPlan_Select(D_ArrivalPlan_Entity dape,D_Arrival_Entity dae,D_Purchase_Entity dpe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                   { "@SoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dape.SoukoCD } },
                   { "@CalcuArrivalPlanDate1", new ValuePair { value1 = SqlDbType.Date, value2 = dape.CalcuArrivalPlanDate1 } },
                   { "@CalcuArrivalPlanDate2", new ValuePair { value1 = SqlDbType.Date, value2 = dape.CalcuArrivalPlanDate2 } },
                   { "@FrmSoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dape.FrmSoukoCD } },
                   { "@ITEMCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dape.ITEMCD } },
                   { "@JanCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dape.JanCD } },
                   { "@SKUCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dape.SKUCD } },
                   { "@MakerItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = dape.MakerItem } },

                   { "@ArrivalDate1", new ValuePair { value1 = SqlDbType.Date, value2 = dae.ArrivalDate1 } },
                   { "@ArrivalDate2", new ValuePair { value1 = SqlDbType.Date, value2 = dae.ArrivalDate2 } },
                   { "@PurchaseSu", new ValuePair { value1 = SqlDbType.Int, value2 = dae.PurchaseSu } },
                   { "@VendorDeliveryNo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dae.VendorDeliveryNo } },

                   { "@PurchaseDateFrom", new ValuePair { value1 = SqlDbType.Date, value2 = dpe.PurchaseDateFrom } },
                   { "@PurchaseDateTo", new ValuePair { value1 = SqlDbType.Date, value2 = dpe.PurchaseDateTo } },
                   { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.VendorCD } },
                   {"@StatusFlg", new ValuePair {value1 = SqlDbType.TinyInt , value2 = dape.statusFlg} },
                   {"@DisplayFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dape.DisplayFlg} },
            };
            UseTransaction = true;
            return SelectData(dic,"D_ArrivalPlan_Select");
        }
    }
}
