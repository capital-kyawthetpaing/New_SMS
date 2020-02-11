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
    public class HacchuuShoukai_BL : Base_BL
    {
        D_Hacchu_DL mdl;
        public HacchuuShoukai_BL()
        {
            mdl = new D_Hacchu_DL();
        }

        /// <summary>
        /// 発注照会にて使用
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable D_Order_SelectAllForShoukai(D_Order_Entity de, M_SKU_Entity mse, string operatorNm, string pc)
        {
            return mdl.D_Order_SelectAllForShoukai(de, mse, operatorNm, pc);
        }
    }
}
