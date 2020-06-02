using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
   public class MasterTouroku_ShiireTanka_DL : Base_DL
    {
        public DataTable M_ItemOrderPrice_Insert(M_ItemOrderPrice_Entity mio,M_ITEM_Entity mi)
        {
            string sp = "M_ItemOrderPrice_Insert";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@vendorcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mio.VendorCD } },
                { "@storecd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mio.StoreCD } },
                { "@changedate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mio.ChangeDate } },
                { "@makerItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mio.MakerItem } },
                { "@display", new ValuePair { value1 = SqlDbType.VarChar, value2 = mio.Display } },
                { "@brandcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mi.BrandCD } },
                { "@sportcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mi.SportsCD} },
                { "@segmentcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mi.SegmentCD } },
                { "@lastyearterm", new ValuePair { value1 = SqlDbType.VarChar, value2 = mi.LastYearTerm } },
                { "@lastseason", new ValuePair { value1 = SqlDbType.VarChar, value2 = mi.LastSeason } },
                { "@heardate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mio.Headerdate } },
                { "@itemcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mio.Display } },
                { "@dateadd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mi.BrandCD } },
                { "@rate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mio.Rate} },
                { "@priceouttax", new ValuePair { value1 = SqlDbType.VarChar, value2 = mi.PriceOutTax } },
                { "@priceoutwithouttax", new ValuePair { value1 = SqlDbType.VarChar, value2 = mio.PriceWithoutTax } },
                { "@insertoperator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mi.LastSeason } },
                { "@insertdatetime", new ValuePair { value1 = SqlDbType.VarChar, value2 = mio.Headerdate } },
                { "@updateoperator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mi.LastSeason } },
                { "@updatetime", new ValuePair { value1 = SqlDbType.VarChar, value2 = mio.Headerdate } },
            };
            return SelectData(dic, sp);
        }
        public DataTable M_ITem_ItemNandPriceoutTax_Select(M_ITEM_Entity mi)
        {
            string sp = "M_ITem_ItemNandPriceoutTax_Select";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ITemCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mi.ITemCD } },
                { "@AddDate", new ValuePair { value1 = SqlDbType.Date, value2 = mi.ChangeDate } },
                
            };
            return SelectData(dic, sp);
        }
    }
}
