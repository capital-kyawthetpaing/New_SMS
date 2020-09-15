using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
namespace DL
{
   public class Search_TenzikaiShouhin_DL:Base_DL
    {
        public DataTable M_Tenzikaishouhin_Search(M_TenzikaiShouhin_Entity mt)
        {
            
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@tenzikainame", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.TenzikaiName } },
                { "@vendorcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.VendorCD } },
                { "@skuname", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.SKUName } },
                { "@year", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.LastYearTerm } },
                { "@season", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.LastSeason } },
                { "@brand", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.BranCDFrom } },
                { "@segment", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.SegmentCDFrom } },
                { "@insertdateF", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.InsertDateFrom } },
                { "@insertdateT", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.InsertDateTo } },
                { "@updatedateF", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.UpdateDateFrom } },
                 { "@updatedateT", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.UpdateDateTo } },
            };

            return SelectData(dic, "M_Tenzikaishouhin_Search");
        }
    }
}
