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
    public class HacchuuShouninNyuuryoku_BL : Base_BL
    {
        D_Hacchu_DL mdl;
        public HacchuuShouninNyuuryoku_BL()
        {
            mdl = new D_Hacchu_DL();
        }

        /// <summary>
        /// 発注承認入力にて使用
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable D_Order_SelectAllForSyonin(D_Order_Entity de)
        {
            return mdl.D_Order_SelectAllForSyonin(de);
        }   
    }
}
