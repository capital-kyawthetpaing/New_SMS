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
    public class ShukkaUriageUpdate_BL : Base_BL
    {
        D_Shipping_DL dsdl;
        public ShukkaUriageUpdate_BL()
        {
            dsdl = new D_Shipping_DL();
        }

        /// <summary>
        /// 出荷売上データ更新照会にて使用
        /// </summary>
        /// <returns></returns>
        public DataTable D_Shipping_SelectAllForShoukai()
        {
            return dsdl.D_Shipping_SelectAllForShoukai();
        }

        public bool M_MultiPorpose_Update(M_MultiPorpose_Entity mme)
        {
            D_Edi_DL ddl= new D_Edi_DL();
            return ddl.M_MultiPorpose_Update(mme);
        }

        public bool ExecUpdate(D_Sales_Entity de)
        {
            D_Sales_DL ddl = new D_Sales_DL();
            return ddl.ShukkaUriageUpdate(de); 
        }
    }
}
