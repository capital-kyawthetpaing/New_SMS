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
    public class SeikyuuSho_BL : Base_BL
    {
        D_Billing_DL mdl;
        public SeikyuuSho_BL()
        {
            mdl = new D_Billing_DL();
        }

        public DataTable D_Billing_SelectForPrint(D_Billing_Entity dbe, M_Customer_Entity mce)
        {
            return mdl.D_Billing_SelectForPrint(dbe, mce);
        }

        /// <summary>
        /// 店舗請求書更新処理
        /// SeikyuuShoよりフラグ更新時に使用
        /// </summary>
        public bool D_Billing_Update(D_Billing_Entity dbe, DataTable dt, string operatorNm, string pc)
        {
            return mdl.D_Billing_Update(dbe, dt, operatorNm, pc);
        }
    }
}
