﻿using System;
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


        public DataTable M_SKU_SelectForSKUCheck(M_SKU_Entity msku)
        {
            string sp = "M_SKU_SelectForSKUCheck";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@skuname", new ValuePair { value1 = SqlDbType.VarChar, value2 = msku.SKUName} },
                { "@colorName", new ValuePair { value1 = SqlDbType.VarChar, value2 = msku.ColorName } },
                { "@sizeName", new ValuePair { value1 = SqlDbType.VarChar, value2 = msku.SizeName } },
                { "@ExhibitionCommomCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = msku.ExhibitionCommonCD } },
                { "@Jancd", new ValuePair { value1 = SqlDbType.VarChar, value2 = msku.JanCD } },
            };
            return SelectData(dic, sp);
        }

        public bool M_Tenzikaishouhin_DeleteUpdate(M_SKU_Entity msku)
        {
            string sp = "M_Tenzikaishouhin_DeleteUpdate";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@changedate", new ValuePair { value1 = SqlDbType.VarChar, value2 = msku.ChangeDate} },
                { "@exhibitionsegmentCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = msku.ExhibitionSegmentCD} },
                { "@year", new ValuePair { value1 = SqlDbType.VarChar, value2 = msku.LastYearTerm} },
                { "@season", new ValuePair { value1 = SqlDbType.VarChar, value2 = msku.LastSeason} },
                { "@skuname", new ValuePair { value1 = SqlDbType.VarChar, value2 = msku.SKUName} },
                { "@colorName", new ValuePair { value1 = SqlDbType.VarChar, value2 = msku.ColorName } },
                { "@sizeName", new ValuePair { value1 = SqlDbType.VarChar, value2 = msku.SizeName } },
                { "@ExhibitionCommomCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = msku.ExhibitionCommonCD } },
                { "@Jancd", new ValuePair { value1 = SqlDbType.VarChar, value2 = msku.JanCD } },
                { "@chkflg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = msku.VariousFLG } },
            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, sp);
        }
    }
}
