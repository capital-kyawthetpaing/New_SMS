﻿using Entity;
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
                { "@brandcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mi.BrandCD } },
                { "@sportcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mi.SportsCD} },
                { "@segmentcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mi.SegmentCD } },
                { "@lastyearterm", new ValuePair { value1 = SqlDbType.VarChar, value2 = mi.LastYearTerm } },
                { "@lastseason", new ValuePair { value1 = SqlDbType.VarChar, value2 = mi.LastSeason } },
                { "@heardate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mi.ChangeDate } },
            };
            return SelectData(dic, sp);
        }
    }
}