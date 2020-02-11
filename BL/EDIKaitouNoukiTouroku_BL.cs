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
    public class EDIKaitouNoukiTouroku_BL : Base_BL
    {
        D_Edi_DL mdl;
        public EDIKaitouNoukiTouroku_BL()
        {
            mdl = new D_Edi_DL();
        }

        /// <summary>
        /// EDI回答納期登録にて使用
        /// </summary>
        /// <returns></returns>
        public DataTable D_Edi_SelectAll()
        {
            return mdl.D_Edi_SelectAll();
        }

        /// <summary>
        /// EDI回答納期登録にて使用
        /// </summary>
        /// <param name="ediNO"></param>
        /// <returns></returns>
        public DataTable D_EDIOrderDetails_SelectAll(D_EDIDetail_Entity de)
        {
            return mdl.D_EDIOrderDetails_SelectAll(de);
        }

        /// <summary>
        /// EDI回答納期登録にて使用
        /// </summary>
        /// <param name="ediOrderNO"></param>
        /// <returns></returns>
        public DataTable D_EDIOrderDetails_SelectForPrint(string ediOrderNO)
        {
            return mdl.D_EDIOrderDetails_SelectForPrint(ediOrderNO);
        }

        public bool M_MultiPorpose_Update(M_MultiPorpose_Entity mme)
        {
            return mdl.M_MultiPorpose_Update(mme);
        }
    }
}
