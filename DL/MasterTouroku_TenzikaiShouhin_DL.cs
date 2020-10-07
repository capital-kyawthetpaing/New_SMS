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

        public DataTable Mastertoroku_Tenzikaishouhin_Select(M_TenzikaiShouhin_Entity mt, string mode)
        {
            string sp = "Mastertoroku_Tenzikaishouhin_Select";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@tenzikainame", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.TanKaCD } },
                { "@vendorcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.VendorCD } },
                { "@lastyear", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.LastYearTerm } },
                { "@lastseason", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.LastSeason } },
                { "@brandcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.BranCDFrom } },
                { "@segment", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.SegmentCDFrom } },
                { "@ctenzikainame", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.TenzikaiName } },
                { "@cvendorcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.VendorCD } },
                { "@clastyear", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.LastYearTerm } },
                { "@clastseason", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.LastSeason } },
                { "@cbrandcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.BranCDFrom } },
                { "@csegment", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.SegmentCDFrom } },
                { "@mode", new ValuePair { value1 = SqlDbType.VarChar, value2 = mode } },


            };
            return SelectData(dic, sp);
        }

    }
}

