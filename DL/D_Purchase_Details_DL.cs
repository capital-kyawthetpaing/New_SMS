using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DL
{
  public  class D_Purchase_Details_DL : Base_DL
    {
        public DataTable ShiireShoukaiDetails_Select (D_Purchase_Details_Entity dpd_e)
        {
             string sp = "D_Purchase_SearchselectAll";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {             
                   { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpd_e.VendorCD  } },
                   { "@JANCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpd_e.JanCD  } },
                   { "@SKUCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpd_e.SKUCD  } },
                   { "@ItemCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpd_e.ItemCD  } },
                   { "@ItemName", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpd_e.ITemName  } },
                   { "@MakerItemCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpd_e.MakerItemCD  } },
                   //{ "@MakerItemCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpd_e.MakerName } },
                   { "@PurchaseSDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpd_e.Purchase_SDate  } },
                   { "@PurchaseEDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpd_e.Purchase_EDate  } },
                   { "@PlanSDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpd_e.Plan_SDate } },
                   { "@PlanEDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpd_e.Plan_EDate  } },
                   { "@OrderSDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpd_e.Order_SDate  } },
                   { "@OrderEDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpd_e.Order_EDate  } },
                    {"@ChkValue",new ValuePair{value1=SqlDbType.TinyInt,value2=dpd_e.CheckValue} },
                   //{ "@ChkSumi", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpd_e.ChkSumi  } },
                  // { "@ChkMi", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpd_e.ChkMi  } },
                   { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpd_e.StaffCD  } },
                   { "@StoreCD", new ValuePair { value1=SqlDbType.VarChar,value2=dpd_e.StoreCD} }

            };
            //return SelectData(dic, "ShiireShoukaiDetails_SelectAll");
            return SelectData(dic, sp);
        }
    }
}
