using GridBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterTouroku_TenzikaiShouhin
{
    public class ClsGridMasterTanzi: ClsGridBase
    {

        internal struct ST_DArray_Grid
        {
            internal string GYONO;
            internal bool Chk;
            internal string TankaCD;
            internal string TenzikaiName; 
            internal string ColorCD;
            internal string ColoarName;
            internal string SizeCD;
            internal string SizeName;
            internal string HanbaiYoteiBi;
            internal string Shiiretanka;
            internal string JoutaiTanka;
            internal string SalePriceOutTax;
            internal string SalePriceOutTax1;
            internal string SalePriceOutTax2;
            internal string SalePriceOutTax3;
            internal string SalePriceOutTax4;
            internal string SalePriceOutTax5;
            internal string BrandCD;
            internal string SegmentCD;
            internal string TaniCD;
            internal string TaxRateFlg;
            internal string Remark;
        }

        internal enum ColNO : int
        {
            GYONO,
            Chk,
            TankaCD,
            TenzikaiName,
            ColorCD,
            ColoarName,
            SizeCD,
            SizeName,
            HanbaiYoteiBi,
            Shiiretanka,
            JoutaiTanka,
            SalePriceOutTax,
            SalePriceOutTax1,
            SalePriceOutTax2,
            SalePriceOutTax3,
            SalePriceOutTax4,
            SalePriceOutTax5,
            BrandCD,
            SegmentCD,
            TaniCD,
            TaxRateFlg,
            Remark,
            Count,
        }
    }
}
