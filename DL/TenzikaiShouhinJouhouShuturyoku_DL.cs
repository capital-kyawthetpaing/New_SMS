using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DL
{
    public class TenzikaiShouhinJouhouShuturyoku_DL : Base_DL
    {
       public DataTable Rpc_TenzikaiShouhinJouhouShuturyoku(M_TenzikaiShouhin_Entity mte)
       {
            string sp = "Rpc_TenzikaiShouhinJouhouShuturyoku";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                {"@VendorCD",new ValuePair{value1=SqlDbType.VarChar,value2=mte.VendorCD} },
                {"@LastYearTerm",new ValuePair{value1=SqlDbType.VarChar,value2=mte.LastYearTerm} },
                {"@LastSeason",new ValuePair{value1=SqlDbType.VarChar,value2=mte.LastSeason} },
                {"@BrandCDFrom",new ValuePair{value1=SqlDbType.VarChar,value2=mte.BranCDFrom} },
                {"@BrandCDTo",new ValuePair{value1=SqlDbType.VarChar,value2=mte.BrandCDTo} },
                {"@SegmentCDFrom",new ValuePair{value1=SqlDbType.VarChar,value2=mte.SegmentCDFrom} },
                {"@SegmentCDTo",new ValuePair{value1=SqlDbType.VarChar,value2=mte.SegmentCDTo} },
                {"@TenzikaiName",new ValuePair{value1=SqlDbType.VarChar,value2=mte.TenzikaiName} },
                {"@JANCD",new ValuePair{value1=SqlDbType.VarChar,value2=mte.JANCD} },
                {"@HanbaiYoteiDateMonth",new ValuePair{value1=SqlDbType.Int,value2=mte.HanbaiYoteiDateMonth} },
                {"@HanbaiYoteiDate",new ValuePair{value1=SqlDbType.VarChar,value2=mte.HanbaiYoteiDate} }
            };
            return SelectData(dic, sp);
       }
    }
}
