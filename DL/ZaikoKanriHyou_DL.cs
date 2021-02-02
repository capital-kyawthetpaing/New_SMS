using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Entity;

namespace DL
{
  public  class ZaikoKanriHyou_DL:Base_DL
    {
        public DataTable RPC_ZaikoKanriHyou(D_Purchase_Details_Entity dpde,D_MonthlyStock_Entity dmse,int chk)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@targetdate", new ValuePair { value1 = SqlDbType.Date, value2 = dpde.ChangeDate } },
                { "@soukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 =dmse.SoukoCD } },
                { "@YYYYMM", new ValuePair { value1 = SqlDbType.Int, value2 =dmse.YYYYMM} },
                { "@itemcd", new ValuePair { value1 = SqlDbType.VarChar, value2 =dpde.ItemCD} },
                { "@sku", new ValuePair { value1 = SqlDbType.VarChar, value2 =dpde.SKUCD } },
                { "@jan", new ValuePair { value1 = SqlDbType.VarChar, value2 =dpde.JanCD } },
                { "@makeritem",new ValuePair{value1=SqlDbType.VarChar,value2=dpde.MakerItemCD} },
                { "@itemName",new ValuePair{value1=SqlDbType.VarChar,value2=dpde.ITemName} },
                { "@purchaseStartDate",new ValuePair{value1=SqlDbType.VarChar,value2=dpde.PurchaseStartDate} },
                { "@purchaseEndDate",new ValuePair{value1=SqlDbType.VarChar,value2=dpde.PurchaseEndDate} },
                { "@related",new ValuePair{value1=SqlDbType.VarChar,value2=chk.ToString()} }
            };
            UseTransaction = true;
            return SelectData(dic, "RPC_ZaikoKanriHyou");
        }

        public DataTable ZaikoKanriHyou_Export(D_Purchase_Details_Entity dpde, D_MonthlyStock_Entity dmse, int chk)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@targetdate", new ValuePair { value1 = SqlDbType.Date, value2 = dpde.ChangeDate } },
                { "@soukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 =dmse.SoukoCD } },
                { "@YYYYMM", new ValuePair { value1 = SqlDbType.Int, value2 =dmse.YYYYMM} },
                { "@itemcd", new ValuePair { value1 = SqlDbType.VarChar, value2 =dpde.ItemCD} },
                { "@sku", new ValuePair { value1 = SqlDbType.VarChar, value2 =dpde.SKUCD } },
                { "@jan", new ValuePair { value1 = SqlDbType.VarChar, value2 =dpde.JanCD } },
                { "@makeritem",new ValuePair{value1=SqlDbType.VarChar,value2=dpde.MakerItemCD} },
                { "@itemName",new ValuePair{value1=SqlDbType.VarChar,value2=dpde.ITemName} },
                { "@purchaseStartDate",new ValuePair{value1=SqlDbType.VarChar,value2=dpde.PurchaseStartDate} },
                { "@purchaseEndDate",new ValuePair{value1=SqlDbType.VarChar,value2=dpde.PurchaseEndDate} },
                { "@related",new ValuePair{value1=SqlDbType.VarChar,value2=chk.ToString()} }
            };
            UseTransaction = true;
            return SelectData(dic, "ZaikoKanriHyou_Export");
        }
    }
}
