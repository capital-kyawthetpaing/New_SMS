using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DL
{
   public class MasterTouroku_TenzikaiHanbaiTankaKakeritu_DL:Base_DL
   {

        string sp = "M_TenzikaiShouhin_Select";
        public DataTable M_TenzikaiShouhin_Select(M_TenzikaiShouhin_Entity mTSE)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@TankaCD", new ValuePair { value1 =SqlDbType.VarChar,value2 = mTSE.TanKaCD } },
                { "@BrandCD", new ValuePair { value1 =SqlDbType.VarChar,value2 = mTSE.BranCDFrom } },
                { "@SegmentCD", new ValuePair { value1 =SqlDbType.VarChar,value2 = mTSE.SegmentCDFrom } },
                { "@LastYearTerm", new ValuePair { value1 =SqlDbType.VarChar,value2 = mTSE.LastYearTerm } },
                { "@LastSeason", new ValuePair { value1 =SqlDbType.VarChar,value2 = mTSE.LastSeason } },
                { "@PriceOutTaxF", new ValuePair { value1 =SqlDbType.VarChar,value2 = mTSE.SalePriceOutTaxF } },
                { "@PriceOutTaxT", new ValuePair { value1 =SqlDbType.VarChar,value2 = mTSE.SalePriceOutTaxT } },
            };
            return SelectData(dic, sp);
        }


        public bool InsertUpdate_TenzikaiHanbaiTankaKakeritu(M_TenzikaiShouhin_Entity mTSE, String xml)
        {
            string sp = "InsertUpdate_TenzikaiHanbaiTankaKakeritu";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@xml", new ValuePair { value1 =SqlDbType.Xml,value2 = xml } },
                { "@TankaCD", new ValuePair { value1 =SqlDbType.VarChar,value2 = mTSE.TanKaCD } },
                { "@BrandCD", new ValuePair { value1 =SqlDbType.VarChar,value2 = mTSE.BranCDFrom } },
                { "@SegmentCD", new ValuePair { value1 =SqlDbType.VarChar,value2 = mTSE.SegmentCDFrom } },
                { "@LastYearTerm", new ValuePair { value1 =SqlDbType.VarChar,value2 = mTSE.LastYearTerm } },
                { "@LastSeason", new ValuePair { value1 =SqlDbType.VarChar,value2 = mTSE.LastSeason } },
                { "@PriceOutTaxF", new ValuePair { value1 =SqlDbType.VarChar,value2 = mTSE.SalePriceOutTaxF } },
                { "@PriceOutTaxT", new ValuePair { value1 =SqlDbType.VarChar,value2 = mTSE.SalePriceOutTaxT } },
                { "@Operator", new ValuePair { value1 =SqlDbType.VarChar,value2 = mTSE.InsertOperator } },
                { "@Program", new ValuePair { value1 =SqlDbType.VarChar,value2 = mTSE.ProgramID } },
                { "@PC", new ValuePair { value1 =SqlDbType.VarChar,value2 = mTSE.PC } },
                { "@OperateMode", new ValuePair { value1 =SqlDbType.VarChar,value2 =mTSE.ProcessMode  } },
                { "@KeyItem", new ValuePair { value1 =SqlDbType.VarChar,value2 = mTSE.Key } },
            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, sp);
        }
    }
}
