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
    public class MailHistoryShoukai_BL : Base_BL
    {
        D_Mail_DL mdl;
        public MailHistoryShoukai_BL()
        {
            mdl = new D_Mail_DL();
        }

        /// <summary>
        /// メール送信照会にて使用
        /// </summary>
        /// <param name="ediNO"></param>
        /// <returns></returns>
        public DataTable D_Mail_SelectAll(D_Mail_Entity de)
        {
            return mdl.D_Mail_SelectAll(de);
        }

        public bool M_MultiPorpose_Update(M_MultiPorpose_Entity mme)
        {
            D_Edi_DL dl = new D_Edi_DL();
            return dl.M_MultiPorpose_Update(mme);
        }

        public DataTable D_MailAddress_SelectAll(D_Mail_Entity de)
        {
            D_MailAddress_DL dl = new D_MailAddress_DL();
            return dl.D_MailAddress_SelectAll(de);
        }

    }
}
