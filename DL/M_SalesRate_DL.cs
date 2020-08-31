using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;

namespace DL
{
    public class M_SalesRate_DL:Base_DL
    {
        public bool SaleRatePrice_InsertUpdate(M_SKU_Entity mskue,M_SKUPrice_Entity mskupe, int mode)
        {
            string sp = "InsertUpdate_HanbaiTankaKakeritu";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@xml", new ValuePair { value1 = SqlDbType.VarChar, value2 = mskue.xml1} },
                { "@StartDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mskue.ChangeDate } },
                { "@EndDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mskue.EndDate } },
                { "@TankaCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mskupe.TankaCD } },
                { "@BrandCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mskue.BrandCD } },             
                { "@ExhibitionSegmentCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mskue.ExhibitionSegmentCD } },               
                { "@LastYearTerm", new ValuePair { value1 = SqlDbType.VarChar, value2 = mskue.LastYearTerm } },              
                { "@LastSeason", new ValuePair { value1 = SqlDbType.VarChar, value2 = mskue.LastSeason } },              
                { "@PriceOutTaxFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = mskue.PriceOutTaxFrom } },
                { "@PriceOutTaxTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = mskue.PriceOutTaxTo } },
                { "@Mode", new ValuePair { value1 = SqlDbType.TinyInt, value2 =mode.ToString() } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = mskue.ProcessMode } },
                { "@ProgramID", new ValuePair { value1 = SqlDbType.VarChar, value2 = mskue.ProgramID} },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mskue.Operator} },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = mskue.PC} },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mskue.Key } },
            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, sp);
        }
    }
}
