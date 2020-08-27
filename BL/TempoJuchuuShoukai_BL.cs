using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using DL;
using System.Data;

namespace BL
{
    public class TempoJuchuuShoukai_BL : Base_BL
    {
        D_Juchuu_DL mdl;
        public TempoJuchuuShoukai_BL()
        {
            mdl = new D_Juchuu_DL();
        }

        /// <summary>
        /// 店舗受注照会にて使用
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable D_Juchu_SelectAllForShoukai(D_Juchuu_Entity de, M_SKU_Entity mse, string operatorNm, string pc)
        {
            return mdl.D_Juchu_SelectAllForShoukai(de, mse, operatorNm, pc);
        }
        /// <summary>
        /// 受注検索にて使用
        /// </summary>
        /// <param name="de"></param>
        /// <param name="mse"></param>
        /// <returns></returns>
        public DataTable D_Juchu_SelectAll(D_Juchuu_Entity de, M_SKU_Entity mse)
        {
            return mdl.D_Juchu_SelectAll(de, mse);
        }
        public DataTable D_Juchu_SelectAllForSearch_JuchuuProcessNO(D_Juchuu_Entity de, M_SKU_Entity mse)
        {
            return mdl.D_Juchu_SelectAllForSearch_JuchuuProcessNO(de, mse);
        }
    }
}
