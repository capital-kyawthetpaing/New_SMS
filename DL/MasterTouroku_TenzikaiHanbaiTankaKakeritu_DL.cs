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

       
        public DataTable M_TenzikaiShouhin_Select(M_TenzikaiShouhin_Entity mTSE)
        {
            string sp = "M_TenzikaiShouhin_Select";
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
        public DataTable M_TenzikaiShouhin_Search(M_TenzikaiShouhin_Entity mte)
        {
            string sp = "M_Tenzikai_Search";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ChangeDate", new ValuePair { value1 =SqlDbType.VarChar,value2 = mte.RefDate } },
                { "@VendorCD", new ValuePair { value1 =SqlDbType.VarChar,value2 = mte.VendorCD } },
                { "@TenzikaiName", new ValuePair { value1 =SqlDbType.VarChar,value2 = mte.TenzikaiName} },
                { "@VendorCDFrom", new ValuePair { value1 =SqlDbType.VarChar,value2 = mte.VendorCDFrom} },
                { "@VendorCDTo", new ValuePair { value1 =SqlDbType.VarChar,value2 = mte.VendorCDTo } },
                { "@LastYearTerm", new ValuePair { value1 =SqlDbType.VarChar,value2 = mte.LastYearTerm } },
                { "@LastSeason", new ValuePair { value1 =SqlDbType.VarChar,value2 = mte.LastSeason } },
                { "@NStartDate", new ValuePair { value1 =SqlDbType.VarChar,value2 = mte.NewRStartDate} },
                { "@NEndDate", new ValuePair { value1 =SqlDbType.VarChar,value2 = mte.NewREndDate} },
                { "@LStartDate", new ValuePair { value1 =SqlDbType.VarChar,value2 = mte.LastCStartDate} },
                { "@LEndDate", new ValuePair { value1 =SqlDbType.VarChar,value2 = mte.LastCEndDate} },
            };
            return SelectData(dic, sp);
        }

        public bool InsertUpdate_TenzikaiHanbaiTankaKakeritu(M_TenzikaiShouhin_Entity mTSE)
        {
            string sp = "InsertUpdate_TenzikaiHanbaiTankaKakeritu";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@xml", new ValuePair { value1 =SqlDbType.Xml,value2 = mTSE.xml1 } },
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
