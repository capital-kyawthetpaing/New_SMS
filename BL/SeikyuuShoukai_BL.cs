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
    public class SeikyuuShoukai_BL : Base_BL
    {
        D_Billing_DL ddl;
        public SeikyuuShoukai_BL()
        {
            ddl = new D_Billing_DL();
        }

        public DataTable D_Billing_SelectAll(D_Billing_Entity dbe, M_Customer_Entity mce)
        {
            return ddl.D_Billing_SelectAll(dbe, mce);
        }
    }
}
