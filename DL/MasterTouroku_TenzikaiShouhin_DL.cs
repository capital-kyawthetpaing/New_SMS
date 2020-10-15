using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DL
{
   public class MasterTouroku_TenzikaiShouhin_DL:Base_DL
    {

        public DataTable Mastertoroku_Tenzikaishouhin_Select(M_TenzikaiShouhin_Entity mt)
        {
            string sp = "Mastertoroku_Tenzikaishouhin_Select";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@tenzikainame", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.TenzikaiName } },
                { "@vendorcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.VendorCD } },
                { "@lastyear", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.LastYearTerm } },
                { "@lastseason", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.LastSeason } },
                { "@brandcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.BranCDFrom } },
                { "@segment", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.SegmentCDFrom } },
            };
            return SelectData(dic, sp);
        }



        public DataTable M_Tenzikaishouhin_SelectForJancd(M_TenzikaiShouhin_Entity mt)
        {
            string sp = "M_Tenzikaishouhin_SelectForJancd";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@tenzikainame", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.TenzikaiName } },
                { "@vendorcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.VendorCD } },
                { "@lastyear", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.LastYearTerm } },
                { "@lastseason", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.LastSeason } },
                { "@brandcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.BranCDFrom } },
                { "@segment", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.SegmentCDFrom } },
                { "@jancd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.JANCD } },
            };
            return SelectData(dic, sp);
        }
    }
}
