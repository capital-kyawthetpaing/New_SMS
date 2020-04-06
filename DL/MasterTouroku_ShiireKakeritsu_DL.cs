using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;

namespace DL
{
   public class MasterTouroku_ShiireKakeritsu_DL:Base_DL
    {
        public DataTable MasterTouroku_ShiireKakeritsu_Select(M_OrderRate_Entity moe)
        {
            string sp = "Temp_M_OrderRate";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                {"@VendorCD", new ValuePair { value1 = System.Data.SqlDbType.VarChar, value2 =moe.VendorCD  } },
                //{"@BrandCD",new ValuePair{value1=System.Data.SqlDbType.VarChar,value2=moe.BrandCD} },
                //{"@SportsCD",new ValuePair{value1=System.Data.SqlDbType.VarChar,value2=moe.SportsCD} },
                //{"@SegmentCD",new ValuePair{value1=System.Data.SqlDbType.VarChar,value2=moe.SegmentCD} },
                //{"@LastSeason",new ValuePair{value1=System.Data.SqlDbType.VarChar,value2=moe.LastSeason} },
                //{"@ChangeDate",new ValuePair{value1=System.Data.SqlDbType.VarChar,value2=moe.ChangeDate} },
                //{"@Rate",new ValuePair{value1=System.Data.SqlDbType.VarChar,value2=moe.Rate} }
            };
            return SelectData(dic, sp);
        }
    }
}
