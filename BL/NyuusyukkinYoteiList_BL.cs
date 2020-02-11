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
    public class NyuusyukkinYoteiList_BL : Base_BL
    {
        D_PaymentConfirm_DL mdl;
        public NyuusyukkinYoteiList_BL()
        {
            mdl = new D_PaymentConfirm_DL();
        }

        public DataTable D_PaymentConfirm_SelectForPrint(D_PaymentConfirm_Entity dce)
        {
            return mdl.D_PaymentConfirm_SelectForPrint(dce);
        }
    }
}
